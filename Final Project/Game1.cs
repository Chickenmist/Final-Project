using Final_Project.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Security.Principal;

namespace Final_Project
{
    //Wilson

    enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Jumping,
        Hurt,
        Dead
    }

    enum BossState
    {
        GroundIdle,
        SlashOne,
        Dash,
        LightningBolt,
        Projectile,
        SlashTwo,
        Jump,
        Hurt,

        Dead
    }

    enum Difficulty
    {
        Human,
        BoneHunter,
        LegendaryBoneKnight,
        MustDie
    }

    enum Screen
    {
        MainMenu,
        Difficulty,
        Controls,
        Information,
        FightScreen,
        WinScreen,
        LoseScreen
    }

    public class Game1 : Game
    {
        Player player;
        Boss boss;
        Projectile projectile;
        PlayerHealthBar playerHealthBar;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Difficulty difficultySelected;

        Screen currentScreen;

        int currentPlayerHealth;

        float timePlayed;
        TimeSpan time;

        float buttonSelectionCooldown;

        KeyboardState keyboardState;
        MouseState mouseState;
        //PlayerState playerState;

        //Player Textures
        List<Texture2D> playerTextures = new List<Texture2D>();
        
        Texture2D playerIdleTexture;
        Texture2D playerWalkTexture;
        Texture2D playerRunTexture;
        Texture2D playerAttackTexture;
        Texture2D playerHurtTexture;
        Texture2D playerDeathTexture;
        Texture2D playerJumpTexture;
        //

        //Boss Tetxures
        List<Texture2D> bossTextures = new List<Texture2D>();

        Texture2D bossIdleTexture;
        Texture2D bossSlashOneTexture;
        Texture2D bossSlashTwoTexture;
        Texture2D bossDashTexture;
        Texture2D bossLightingAttackTexture;
        Texture2D bossProjectileTexture;

        Texture2D bossJumpTexture;
        Texture2D bossHurtTexture;
        Texture2D bossDeadTexture;
        //

        //Projectile texture
        Texture2D projectileTexture;
        //

        //Background and floor
        Rectangle background;
        Texture2D battleBackgroundTexture;
        Texture2D titleCardTexture;
        Rectangle floor;
        //

        //Button Textures
        Texture2D playUpTexture;
        Texture2D playDownTexture;
        Texture2D controlsUpTexture;
        Texture2D controlsDownTexture;
        Texture2D creditsUpTexture;
        Texture2D creditsDownTexture;


        Texture2D quitUpTexture;
        Texture2D quitDownTexture;

        Texture2D humanUpTexture;
        Texture2D humanDownTexture;
        Texture2D boneHunterUpTexture;
        Texture2D boneHunterDownTexture;
        Texture2D lBKUpTexture;
        Texture2D lBKDownTexture;
        Texture2D mustDieUpTexture;
        Texture2D mustDieDownTexture;
        //

        //Button rectangles
        Rectangle startRectangle;
        Rectangle controlsRectangle;
        Rectangle creditsRectangle;

        Rectangle quitRectangle;

        Rectangle humanRectangle;
        Rectangle boneHunterRectangle;
        Rectangle lBKRectangle;
        Rectangle mustDieRectangle;
        //

        //Information Textures
        Texture2D returnTexture;
        Texture2D viewCtrlTexture;
        Texture2D quitGameTexture;
        Texture2D viewInfoTexture;
        Texture2D musicUsedTexture;
        Texture2D ctrlsTexture;

        Texture2D selectDiffTexture;
        Texture2D humanInfoTexture;
        Texture2D boneHuntInfoTexture;
        Texture2D lBKInfoTexture;
        Texture2D mustDieInfoTexture;
        //

        //Information rectangle
        Rectangle informationRectangle;
        //

        //Variables to check which button the mouse is hovering
        bool hoverPlay;
        bool hoverControls;
        bool hoverCredits;

        bool hoverQuit;

        bool hoverHuman;
        bool hoverBoneHunter;
        bool hoverLBK;
        bool hoverMustDie;
        //

        //Game over texture and rectangle
        Texture2D gameOverTexture;
        Rectangle gameOverRectangle;
        //

        //Sound Effects
        SoundEffect playerSlashSFX; //Player attack sfx
        SoundEffectInstance playerSlashInstance;

        SoundEffect bossAttackSFX; //Boss attack sfx
        SoundEffectInstance bossAttackInstance;

        SoundEffect menuSoundSFX; //Menu sfx
        SoundEffectInstance menuSoundInstance;

        SoundEffect howCouldSFX; //Boss dies
        SoundEffectInstance howCouldInstance;

        SoundEffect scumSFX; //Boss is damaged
        SoundEffectInstance scumInstance;

        SoundEffect warmUpSFX; //Boss wins on Human
        SoundEffectInstance warmUpInstance;

        SoundEffect wasteOfTimeSFX; //Boss wins on Bone Hunter
        SoundEffectInstance wasteOfTimeInstance;

        SoundEffect patheticSFX; //Bons wins on LBK
        SoundEffectInstance patheticInstance;

        SoundEffect disapointingSFX; //Boss wins on Must Die
        SoundEffectInstance disapointingInstance;
        //

        //Bools to check if a sound has been played
        bool playerAttackPlayed;
        bool bossAttackPlayed;
        bool scumPlayed;
        //

        //Fonts
        SpriteFont timeTaken;
        SpriteFont titleFont;
        //

        //Music
        Song menuSong;
        Song battleSong;
        Song winSong;
        Song loseSong;
        //

        //This is for box checking
        Texture2D rectangleTexture;
        //

        //Health Bar Textures
        Texture2D playerHealthTexture;
        //

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 540;
            _graphics.ApplyChanges();

            background = new Rectangle(0, 0, 960, 540);

            floor = new Rectangle(0, 490, 960, 50);

            currentScreen = Screen.MainMenu;

            //Difficulty button rectangles
            humanRectangle = new Rectangle(370, 30, 128, 32);
            boneHunterRectangle = new Rectangle(370, 64, 128, 32);
            lBKRectangle = new Rectangle(370, 98, 128, 32);
            mustDieRectangle = new Rectangle(370, 132, 128, 32);
            //

            //Main menu button rectangles
            startRectangle = new Rectangle(370, 64, 128, 32);
            controlsRectangle = new Rectangle(370, 98, 128, 32);
            creditsRectangle = new Rectangle(370, 132, 128, 32);
            //

            quitRectangle = new Rectangle(370, 166, 128, 32);

            informationRectangle = new Rectangle(253, 215, 380, 295);

            gameOverRectangle = new Rectangle(170, 40, 620, 162);

            MediaPlayer.Volume = 0.3f;

            timePlayed = 0;

            base.Initialize();
            
            //Adds the player sprites to the texture list and creates the player
            playerTextures.Add(playerIdleTexture);
            playerTextures.Add(playerWalkTexture);
            playerTextures.Add(playerRunTexture);
            playerTextures.Add(playerAttackTexture);
            playerTextures.Add(playerJumpTexture);
            playerTextures.Add(playerHurtTexture);
            playerTextures.Add(playerDeathTexture);
            player = new Player(playerTextures, 50, floor.Y - 95);
            //

            //Adds the boss sprites to the texture list and creates the boss
            bossTextures.Add(bossIdleTexture);
            bossTextures.Add(bossSlashOneTexture);
            bossTextures.Add(bossDashTexture);
            bossTextures.Add(bossLightingAttackTexture);
            bossTextures.Add(bossProjectileTexture);
            bossTextures.Add(bossSlashTwoTexture);
            bossTextures.Add(bossJumpTexture);
            bossTextures.Add(bossHurtTexture);
            bossTextures.Add(bossDeadTexture);

            boss = new Boss(bossTextures, 830, floor.Y - 105);
            //

            //Creates the projectile
            projectile = new Projectile(projectileTexture, 0, 0);
            //

            //Creates the health bars
            playerHealthBar = new PlayerHealthBar(playerHealthTexture, 0, 0);
            //

            MediaPlayer.Play(menuSong);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //Load Player Sprites
            playerAttackTexture = Content.Load<Texture2D>("Skeleton Attack");
            playerIdleTexture = Content.Load<Texture2D>("Skeleton Idle");
            playerHurtTexture = Content.Load<Texture2D>("Skeleton Hurt");
            playerDeathTexture = Content.Load<Texture2D>("Skeleton Dead");
            playerRunTexture = Content.Load<Texture2D>("Skeleton Run");
            playerWalkTexture = Content.Load<Texture2D>("Skeleton Walk");
            playerJumpTexture = Content.Load<Texture2D>("Skeleton Jumping");
            //

            //Load Boss Sprites
            bossIdleTexture = Content.Load<Texture2D>("Boss Ground Idle");
            bossSlashOneTexture = Content.Load<Texture2D>("Boss Attack 1");
            bossSlashTwoTexture = Content.Load<Texture2D>("Boss Attack 2");
            bossDashTexture = Content.Load<Texture2D>("Boss Dash");
            bossLightingAttackTexture = Content.Load<Texture2D>("Lightning Bolt");
            bossProjectileTexture = Content.Load<Texture2D>("Boss Projectile");
            bossJumpTexture = Content.Load<Texture2D>("Boss Jump");
            bossHurtTexture = Content.Load<Texture2D>("Boss Hurt");
            bossDeadTexture = Content.Load<Texture2D>("Boss Dead");
            //

            //loads projectile texture
            projectileTexture = Content.Load<Texture2D>("Projectile Sprite");
            //

            //Load Background Sprites
            battleBackgroundTexture = Content.Load<Texture2D>("Background");
            titleCardTexture = Content.Load<Texture2D>("Title Card");
            //

            //Load Button Sprites
            playUpTexture = Content.Load<Texture2D>("Start Up Texture");
            playDownTexture = Content.Load<Texture2D>("Start Down Texture");
            controlsUpTexture = Content.Load<Texture2D>("Controls Up");
            controlsDownTexture = Content.Load<Texture2D>("Controls Down");
            creditsUpTexture = Content.Load<Texture2D>("Credits Up");
            creditsDownTexture = Content.Load<Texture2D>("Credits Down");


            quitUpTexture = Content.Load<Texture2D>("Quit Up Sprite");
            quitDownTexture = Content.Load<Texture2D>("Quit Down Sprite");

            humanUpTexture = Content.Load<Texture2D>("Human Up");
            humanDownTexture = Content.Load<Texture2D>("Human Down");
            boneHunterUpTexture = Content.Load<Texture2D>("Bone Hunter Up");
            boneHunterDownTexture = Content.Load<Texture2D>("Bone Hunter Down");
            lBKUpTexture = Content.Load<Texture2D>("LBK Up");
            lBKDownTexture = Content.Load<Texture2D>("LBK Down");
            mustDieUpTexture = Content.Load<Texture2D>("Must Die Up");
            mustDieDownTexture = Content.Load<Texture2D>("Must Die Down");
            //

            //Loads Information Boxes
            selectDiffTexture = Content.Load<Texture2D>("Select Difficulty");
            humanInfoTexture = Content.Load<Texture2D>("Human Description");
            boneHuntInfoTexture = Content.Load<Texture2D>("Bone Hunter Description");
            lBKInfoTexture = Content.Load<Texture2D>("LBK Description");
            mustDieInfoTexture = Content.Load<Texture2D>("Must Die Description");

            returnTexture = Content.Load<Texture2D>("Return To Main Menu");
            musicUsedTexture = Content.Load<Texture2D>("Music Used");
            viewCtrlTexture = Content.Load<Texture2D>("View Controls");
            ctrlsTexture = Content.Load<Texture2D>("Controls");
            viewInfoTexture = Content.Load<Texture2D>("View Information");
            quitGameTexture = Content.Load<Texture2D>("Quit The Game");
            //

            //Load game over and victory
            gameOverTexture = Content.Load<Texture2D>("Game Over");
            //

            //Loads sound effects
            playerSlashSFX = Content.Load<SoundEffect>("Sword Slash");
            playerSlashInstance = playerSlashSFX.CreateInstance();

            bossAttackSFX = Content.Load<SoundEffect>("Boss Sound Effect");
            bossAttackInstance = bossAttackSFX.CreateInstance();

            menuSoundSFX = Content.Load<SoundEffect>("Menu Sound");
            menuSoundInstance = menuSoundSFX.CreateInstance();

            howCouldSFX = Content.Load<SoundEffect>("How Could I lose");
            howCouldInstance = howCouldSFX.CreateInstance();

            scumSFX = Content.Load<SoundEffect>("Scum");
            scumInstance = scumSFX.CreateInstance();

            warmUpSFX = Content.Load<SoundEffect>("Warm Up");
            warmUpInstance = warmUpSFX.CreateInstance();

            wasteOfTimeSFX = Content.Load<SoundEffect>("Wasting My Time");
            wasteOfTimeInstance = wasteOfTimeSFX.CreateInstance();

            patheticSFX = Content.Load<SoundEffect>("Pathetic");
            patheticInstance = patheticSFX.CreateInstance();

            disapointingSFX = Content.Load<SoundEffect>("Disapointing");
            disapointingInstance = disapointingSFX.CreateInstance();
            //

            //Loads Music
            menuSong = Content.Load<Song>("DMC 3 Mission Start");
            battleSong = Content.Load<Song>("Bloody Tears");
            winSong = Content.Load<Song>("Reflection Mission Clear");
            loseSong = Content.Load<Song>("End of the World - Gameover");
            //

            //Loads fonts
            timeTaken = Content.Load<SpriteFont>("Time Font");
            titleFont = Content.Load<SpriteFont>("Title");
            //

            //Loads health bars
            playerHealthTexture = Content.Load<Texture2D>("");
            //

            //Loads the rectangle texture for box checking
            rectangleTexture = Content.Load<Texture2D>("rectangle");
            //
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            MediaPlayer.Volume = 0.3f;

            MediaPlayer.IsRepeating = true;

            Window.Title = $"{mouseState.X} {mouseState.Y}";

            if (buttonSelectionCooldown > 0) //Button selection cooldown so that options don't get picked automatically when changing screens
            {
                buttonSelectionCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (buttonSelectionCooldown <= 0)
            {
                buttonSelectionCooldown = 0;
            }

            if (currentScreen == Screen.MainMenu) //The main menu
            {
                if (startRectangle.Contains(mouseState.Position)) //Mouse is hovering the play button
                {
                    hoverPlay = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //Play is selected
                    {
                        menuSoundInstance.Play();

                        buttonSelectionCooldown = 0.1f;

                        currentScreen = Screen.Difficulty;
                    }
                }
                else //Mouse is not hovering the play button
                {
                    hoverPlay = false;
                }

                if (controlsRectangle.Contains(mouseState.Position)) //Mouse is hovering the contols button
                {
                    hoverControls = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //Controls is selected
                    {
                        menuSoundInstance.Play();

                        currentScreen = Screen.Controls;
                    }
                }
                else //Mouse isn't hovering the controls button
                {
                    hoverControls = false;
                }

                if (creditsRectangle.Contains(mouseState.Position)) //Mouse is hovering the credits button
                {
                    hoverCredits = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //Credits is selected
                    {
                        menuSoundInstance.Play();

                        currentScreen = Screen.Information;
                    }
                }
                else //Mouse isn't hovering the credits button
                {
                    hoverCredits = false;
                }

                if (quitRectangle.Contains(mouseState.Position)) //The mouse is hovering the Quit button
                {
                    hoverQuit = true;

                    if (buttonSelectionCooldown == 0) // this is to keep it from automatically quitting the game if you return to the main menu
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed) //Quit is selected
                        {
                            menuSoundInstance.Play();

                            MediaPlayer.Stop();

                            this.Exit();
                        }
                    }
                }
                else //The mouse isn't hovering the Quit button
                {
                    hoverQuit = false;
                }
            }
            else if (currentScreen == Screen.Difficulty) //Difficulty selection screen
            {
                if (humanRectangle.Contains(mouseState.Position)) //The mouse is hovering the Human button
                {
                    hoverHuman = true;

                    if (buttonSelectionCooldown == 0)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Human difficulty
                        {
                            menuSoundInstance.Play();

                            boss.GetDifficulty = 1;
                            player.GetDifficulty = 1;

                            difficultySelected = Difficulty.Human;

                            MediaPlayer.Stop();

                            MediaPlayer.Play(battleSong);

                            currentScreen = Screen.FightScreen;
                        }
                    }
                }
                else //The mouse isn't hovering the Human button
                {
                    hoverHuman = false;
                }

                if (boneHunterRectangle.Contains(mouseState.Position)) //The mouse if hovering over the Bone Hunter button
                {
                    hoverBoneHunter = true;

                    if (buttonSelectionCooldown == 0)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Bone Hunter difficulty
                        {
                            menuSoundInstance.Play();

                            boss.GetDifficulty = 2;
                            player.GetDifficulty = 2;

                            difficultySelected = Difficulty.BoneHunter;

                            MediaPlayer.Stop();

                            MediaPlayer.Play(battleSong);

                            currentScreen = Screen.FightScreen;
                        }
                    }
                }
                else //The mouse isn't hovering the Bone Hunter button
                {
                    hoverBoneHunter = false;
                }

                if (lBKRectangle.Contains(mouseState.Position)) //The mouse is hovering the Legendary Bone Knight button 
                {
                    hoverLBK = true;

                    if (buttonSelectionCooldown == 0)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Legendary Bone Knight difficulty
                        {
                            menuSoundInstance.Play();

                            boss.GetDifficulty = 3;
                            player.GetDifficulty = 3;

                            difficultySelected = Difficulty.LegendaryBoneKnight;

                            MediaPlayer.Stop();

                            MediaPlayer.Play(battleSong);

                            currentScreen = Screen.FightScreen;
                        }
                    }
                }
                else //The mouse isn't hovering the Legendary Bone Knoght difficulty
                {
                    hoverLBK = false;
                }

                if (mustDieRectangle.Contains(mouseState.Position)) //The mouse is hovering the Must Die button
                {
                    hoverMustDie = true;

                    if (buttonSelectionCooldown == 0)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Must Die difficulty
                        {
                            menuSoundInstance.Play();

                            boss.GetDifficulty = 4;
                            player.GetDifficulty = 4;

                            difficultySelected = Difficulty.MustDie;

                            MediaPlayer.Stop();

                            MediaPlayer.Play(battleSong);

                            currentScreen = Screen.FightScreen;
                        }
                    }
                }
                else //The mouse isn't hovering the Must Die button
                {
                    hoverMustDie = false;
                }

                if (quitRectangle.Contains(mouseState.Position)) //The mouse is hovering the Quit button
                {
                    hoverQuit = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //Quit is selected
                    {
                        menuSoundInstance.Play();

                        buttonSelectionCooldown = 0.1f;
                        currentScreen = Screen.MainMenu;
                    }
                }
                else //The mouse isn't hovering the Quit button
                {
                    hoverQuit = false;
                }
            }
            else if (currentScreen == Screen.Controls) //Controls screen
            {
                if (quitRectangle.Contains(mouseState.Position)) //The mouse is hovering the Quit button
                {
                    hoverQuit = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //Quit is selected
                    {
                        menuSoundInstance.Play();

                        buttonSelectionCooldown = 0.1f;
                        currentScreen = Screen.MainMenu;
                    }
                }
                else //The mouse isn't hovering the Quit button
                {
                    hoverQuit = false;
                }
            }
            else if (currentScreen == Screen.Information) //Information screen
            {
                if (quitRectangle.Contains(mouseState.Position)) //The mouse is hovering the Quit button
                {
                    hoverQuit = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //Quit is selected
                    {
                        menuSoundInstance.Play();

                        buttonSelectionCooldown = 0.1f;
                        currentScreen = Screen.MainMenu;
                    }
                }
                else //The mouse isn't hovering the Quit button
                {
                    hoverQuit = false;
                }
            }
            else if (currentScreen == Screen.FightScreen) //Fight Screen
            {
                player.Update(gameTime, keyboardState, mouseState, boss);

                boss.Update(gameTime, player, projectile);

                projectile.Update(gameTime, player, boss);

                timePlayed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                time = TimeSpan.FromSeconds(timePlayed);

                if (boss.damaged && !scumPlayed) //Plays the boss sound effect when hit
                {
                    scumInstance.Play();
                    scumPlayed = true;
                }
                else if (!boss.damaged && scumPlayed) //Resets the sound effect for the next time the boss is hit
                {
                    scumPlayed = false;
                }

                if (player.attacking && !playerAttackPlayed) //Plays the sound effect for the player attack
                {
                    playerSlashInstance.Play();
                    playerAttackPlayed = true;
                }
                else if (!player.attacking && playerAttackPlayed) //Resets the sound effect for the next time the player attacks
                {
                    playerAttackPlayed = false;
                }

                if (boss.attacking && !bossAttackPlayed)
                {
                    bossAttackInstance.Play();
                    bossAttackPlayed = true;
                }
                else if (!boss.attacking && bossAttackPlayed)
                {
                    bossAttackPlayed = false;
                }

                if (player.playerDead) //Player died
                {
                    MediaPlayer.Stop();

                    if (difficultySelected == Difficulty.Human)
                    {
                        warmUpInstance.Play();
                    }
                    else if (difficultySelected == Difficulty.BoneHunter)
                    {
                        wasteOfTimeInstance.Play();
                    }
                    else if (difficultySelected == Difficulty.LegendaryBoneKnight)
                    {
                        patheticInstance.Play();
                    }
                    else if (difficultySelected == Difficulty.MustDie)
                    {
                        disapointingInstance.Play();
                    }


                    currentScreen = Screen.LoseScreen;
                }
                else if (boss.bossDead) //Boss died
                {
                    MediaPlayer.Stop();

                    howCouldInstance.Play();

                    currentScreen = Screen.WinScreen;
                }
            }
            else if (currentScreen == Screen.WinScreen) //Win screen
            {

                if (MediaPlayer.State == MediaState.Stopped && howCouldInstance.State == SoundState.Stopped)
                {
                    MediaPlayer.Play(winSong);
                }

                if (quitRectangle.Contains(mouseState.Position)) //The mouse is hovering the Quit button
                {
                    hoverQuit = true;

                    if (buttonSelectionCooldown == 0) // this is to keep it from automatically quitting the game if you return to the main menu
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed) //Quit is selected
                        {
                            menuSoundInstance.Play();

                            MediaPlayer.Stop();

                            this.Exit();
                        }
                    }
                }
                else //The mouse isn't hovering the Quit button
                {
                    hoverQuit = false;
                }
            }
            else if (currentScreen == Screen.LoseScreen) //Lose screen
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    if (difficultySelected == Difficulty.Human && warmUpInstance.State == SoundState.Stopped)
                    {
                        MediaPlayer.Play(loseSong);
                    }
                    else if (difficultySelected == Difficulty.BoneHunter && wasteOfTimeInstance.State == SoundState.Stopped)
                    {
                        MediaPlayer.Play(loseSong);
                    }
                    else if (difficultySelected == Difficulty.LegendaryBoneKnight && patheticInstance.State == SoundState.Stopped)
                    {
                        MediaPlayer.Play(loseSong);
                    }
                    else if (difficultySelected == Difficulty.MustDie && disapointingInstance.State == SoundState.Stopped)
                    {
                        MediaPlayer.Play(loseSong);
                    }
                }

                if (quitRectangle.Contains(mouseState.Position)) //The mouse is hovering the Quit button
                {
                    hoverQuit = true;

                    if (buttonSelectionCooldown == 0) // this is to keep it from automatically quitting the game if you return to the main menu
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed) //Quit is selected
                        {
                            menuSoundInstance.Play();

                            MediaPlayer.Stop();

                            this.Exit();
                        }
                    }
                }
                else //The mouse isn't hovering the Quit button
                {
                    hoverQuit = false;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (currentScreen == Screen.MainMenu) //Main menu
            {
                _spriteBatch.Draw(titleCardTexture, background, Color.White);

                _spriteBatch.DrawString(titleFont, "Bones May Cry", new Vector2(335, 5), Color.White);

                if(hoverPlay) //Hovering the play button
                {
                    _spriteBatch.Draw(playDownTexture, startRectangle, Color.White);
                    _spriteBatch.Draw(selectDiffTexture, informationRectangle, Color.White);
                }
                else //Not hovering the play button
                {
                    _spriteBatch.Draw(playUpTexture, startRectangle, Color.White);
                }

                if (hoverControls) //Hovering the controls button
                {
                    _spriteBatch.Draw(controlsDownTexture, controlsRectangle, Color.White);
                    _spriteBatch.Draw(viewCtrlTexture, informationRectangle, Color.White);
                }
                else //Not hovering the controls button
                {
                    _spriteBatch.Draw(controlsUpTexture, controlsRectangle, Color.White);
                }

                if(hoverCredits) //Hovering the credits button
                {
                    _spriteBatch.Draw(creditsDownTexture, creditsRectangle, Color.White);
                    _spriteBatch.Draw(viewInfoTexture, informationRectangle, Color.White);
                }
                else //Not hovering the credits button
                {
                    _spriteBatch.Draw(creditsUpTexture, creditsRectangle, Color.White);
                }

                if (hoverQuit) //Hovering the quit button
                {
                    _spriteBatch.Draw(quitDownTexture, quitRectangle, Color.White);
                    _spriteBatch.Draw(quitGameTexture, informationRectangle, Color.White);
                }
                else //Not hovering the quit button 
                {
                    _spriteBatch.Draw(quitUpTexture, quitRectangle, Color.White);
                }
            }
            else if (currentScreen == Screen.Difficulty) //Difficulty selection screen
            {
                _spriteBatch.Draw(titleCardTexture, background, Color.White);

                if (hoverHuman) //Hovering the Human button
                {
                    _spriteBatch.Draw(humanDownTexture, humanRectangle, Color.White);
                    _spriteBatch.Draw(humanInfoTexture, informationRectangle, Color.White);
                }
                else //Not hovering the human button
                {
                    _spriteBatch.Draw(humanUpTexture, humanRectangle, Color.White);
                }

                if (hoverBoneHunter) //Hovering the Bone Hunter button
                {
                    _spriteBatch.Draw(boneHunterDownTexture, boneHunterRectangle, Color.White);
                    _spriteBatch.Draw(boneHuntInfoTexture, informationRectangle, Color.White);
                }
                else //Not hovering the Bone Hunter button
                {
                    _spriteBatch.Draw(boneHunterUpTexture, boneHunterRectangle, Color.White);
                }

                if (hoverLBK) //Hovering the Legendary Bone Knight button
                {
                    _spriteBatch.Draw(lBKDownTexture, lBKRectangle, Color.White);
                    _spriteBatch.Draw(lBKInfoTexture, informationRectangle, Color.White);
                }
                else //Not hovering the Legendary Bone Knight button
                {
                    _spriteBatch.Draw(lBKUpTexture, lBKRectangle, Color.White);
                }

                if (hoverMustDie) //Hovering the Must Die button
                {
                    _spriteBatch.Draw(mustDieDownTexture, mustDieRectangle, Color.White);
                    _spriteBatch.Draw(mustDieInfoTexture, informationRectangle, Color.White);
                }
                else //Not hovering the Must Die button
                {
                    _spriteBatch.Draw(mustDieUpTexture, mustDieRectangle, Color.White);
                }

                if (hoverQuit) //Hovering the quit button
                {
                    _spriteBatch.Draw(quitDownTexture, quitRectangle, Color.White);
                    _spriteBatch.Draw(returnTexture, informationRectangle, Color.White);
                }
                else //Not hovering the quit button 
                {
                    _spriteBatch.Draw(quitUpTexture, quitRectangle, Color.White);
                }

                if (!hoverBoneHunter && !hoverHuman && !hoverLBK && !hoverMustDie && !hoverQuit)
                {
                    _spriteBatch.Draw(selectDiffTexture, informationRectangle, Color.White);
                }
            }
            else if (currentScreen == Screen.Controls) //Controls screen
            {
                _spriteBatch.Draw(titleCardTexture, background, Color.White);

                if (hoverQuit) //Hovering the quit button
                {
                    _spriteBatch.Draw(quitDownTexture, quitRectangle, Color.White);
                    _spriteBatch.Draw(returnTexture, informationRectangle, Color.White);
                }
                else //Not hovering the quit button 
                {
                    _spriteBatch.Draw(quitUpTexture, quitRectangle, Color.White);

                    _spriteBatch.Draw(ctrlsTexture, informationRectangle, Color.White);
                }
            }
            else if (currentScreen == Screen.Information) //Information screen
            {
                _spriteBatch.Draw(titleCardTexture, background, Color.White);

                if (hoverQuit) //Hovering the quit button
                {
                    _spriteBatch.Draw(quitDownTexture, quitRectangle, Color.White);
                    _spriteBatch.Draw(returnTexture, informationRectangle, Color.White);
                }
                else //Not hovering the quit button 
                {
                    _spriteBatch.Draw(quitUpTexture, quitRectangle, Color.White);

                    _spriteBatch.Draw(musicUsedTexture, informationRectangle, Color.White);
                }
            }
            else if (currentScreen == Screen.FightScreen) //Fight screen
            {
                _spriteBatch.Draw(battleBackgroundTexture, background, Color.White);

                player.Draw(_spriteBatch);
                _spriteBatch.Draw(rectangleTexture, player.playerHurtbox, new Color(Color.Black, 0.5f));
                _spriteBatch.Draw(rectangleTexture, player.playerHitbox, new Color(Color.Red, 0.5f));

                boss.Draw(_spriteBatch);
                _spriteBatch.Draw(rectangleTexture, boss.bossHurtbox, new Color(Color.Black, 0.5f));
                _spriteBatch.Draw(rectangleTexture, boss.bossHitbox, new Color(Color.Red, 0.5f));

                projectile.Draw(_spriteBatch);
            }
            else if (currentScreen == Screen.WinScreen) //Win screen
            {
                GraphicsDevice.Clear(Color.Black);

                boss.Draw(_spriteBatch);

                _spriteBatch.DrawString(timeTaken, $"You Took {time.ToString("mm':'ss")} to Win", new Vector2(0, 0), Color.White);

                _spriteBatch.DrawString(titleFont, "Cleared", new Vector2(377, 80), Color.White);
                if (hoverQuit) //Hovering the quit button
                {
                    _spriteBatch.Draw(quitDownTexture, quitRectangle, Color.White);
                }
                else //Not hovering the quit button 
                {
                    _spriteBatch.Draw(quitUpTexture, quitRectangle, Color.White);
                }
            }
            else if (currentScreen == Screen.LoseScreen) //Lose screen
            {
                GraphicsDevice.Clear(Color.Black);

                player.Draw(_spriteBatch);

                _spriteBatch.Draw(gameOverTexture, gameOverRectangle, Color.White);
                _spriteBatch.DrawString(timeTaken, $"You Took {time.ToString("mm':'ss")} to Die", new Vector2(0, 0), Color.White);

                if (hoverQuit) //Hovering the quit button
                {
                    _spriteBatch.Draw(quitDownTexture, quitRectangle, Color.White);
                }
                else //Not hovering the quit button 
                {
                    _spriteBatch.Draw(quitUpTexture, quitRectangle, Color.White);
                }
            }

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}