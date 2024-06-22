using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Difficulty difficultySelected;

        Screen currentScreen;

        int currentPlayerHealth;

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

        Rectangle quitRectangle;

        Rectangle humanRectangle;
        Rectangle boneHunterRectangle;
        Rectangle lBKRectangle;
        Rectangle mustDieRectangle;
        //

        //Variables to check which button the mouse is hovering

        bool hoverQuit;

        bool hoverHuman;
        bool hoverBoneHunter;
        bool hoverLBK;
        bool hoverMustDie;
        //

        //This is for box checking
        Texture2D rectangleTexture;
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

            currentScreen = Screen.Difficulty;

            humanRectangle = new Rectangle(370, 30, 128, 32);
            boneHunterRectangle = new Rectangle(370, 64, 128, 32);
            lBKRectangle = new Rectangle(370, 96, 128, 32);
            mustDieRectangle = new Rectangle(370, 128, 128, 32);
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


            humanUpTexture = Content.Load<Texture2D>("Human Up");
            humanDownTexture = Content.Load<Texture2D>("Human Down");
            boneHunterUpTexture = Content.Load<Texture2D>("Bone Hunter Up");
            boneHunterDownTexture = Content.Load<Texture2D>("Bone Hunter Down");
            lBKUpTexture = Content.Load<Texture2D>("LBK Up");
            lBKDownTexture = Content.Load<Texture2D>("LBK Down");
            mustDieUpTexture = Content.Load<Texture2D>("Must Die Up");
            mustDieDownTexture = Content.Load<Texture2D>("Must Die Down");
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

            Window.Title = $"{mouseState.X} {mouseState.Y}";

            if (currentScreen == Screen.MainMenu) //The main menu
            {

            }
            else if (currentScreen == Screen.Difficulty) //Difficulty selection screen
            {
                if (humanRectangle.Contains(mouseState.Position)) //The mouse is hovering the Human button
                {
                    hoverHuman = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Human difficulty
                    {
                        boss.GetDifficulty = 1;
                        player.GetDifficulty = 1;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else //The mouse isn't hovering the Human button
                {
                    hoverHuman = false;
                }
                
                if (boneHunterRectangle.Contains(mouseState.Position)) //The mouse if hovering over the Bone Hunter button
                {
                    hoverBoneHunter = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Bone Hunter difficulty
                    {
                        boss.GetDifficulty = 2;
                        player.GetDifficulty = 2;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else //The mouse isn't hovering the Bone Hunter button
                {
                    hoverBoneHunter = false;
                }
                
                if (lBKRectangle.Contains(mouseState.Position)) //The mouse is hovering the Legendary Bone Knight button 
                {
                    hoverLBK = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Legendary Bone Knight difficulty
                    {
                        boss.GetDifficulty = 3;
                        player.GetDifficulty = 3;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else //The mouse isn't hovering the Legendary Bone Knoght difficulty
                {
                    hoverLBK = false;
                }
                
                if (mustDieRectangle.Contains(mouseState.Position)) //The mouse is hovering the Must Die button
                {
                    hoverMustDie = true;

                    if (mouseState.LeftButton == ButtonState.Pressed) //The player selects the Must Die difficulty
                    {
                        boss.GetDifficulty = 4;
                        player.GetDifficulty = 4;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else //The mouse isn't hovering the Must Die button
                {
                    hoverMustDie = false;
                }
            }
            else if (currentScreen == Screen.Controls) //Controls screen
            {

            }
            else if (currentScreen == Screen.Information) //Information screen
            {

            }
            else if (currentScreen == Screen.FightScreen) //Fight Screen
            {   
                player.Update(gameTime, keyboardState, mouseState, boss);

                boss.Update(gameTime, player, projectile);

                projectile.Update(gameTime, player, boss);

                if (player.playerDead)
                {
                    currentScreen = Screen.LoseScreen;
                }
                else if (boss.bossDead)
                {
                    currentScreen = Screen.WinScreen;
                }
            }
            else if (currentScreen == Screen.WinScreen) //win screen
            {

            }
            else if (currentScreen == Screen.LoseScreen) //Lose screen
            {

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
            }
            else if (currentScreen == Screen.Difficulty) //Difficulty selection screen
            {
                _spriteBatch.Draw(titleCardTexture, background, Color.White);

                if (hoverHuman) //Hovering the Human button
                {
                    _spriteBatch.Draw(humanDownTexture, humanRectangle, Color.White);
                }
                else //Not hovering the human button
                {
                    _spriteBatch.Draw(humanUpTexture, humanRectangle, Color.White);
                }

                if (hoverBoneHunter) //Hovering the Bone Hunter button
                {
                    _spriteBatch.Draw(boneHunterDownTexture, boneHunterRectangle, Color.White);
                }
                else //Not hovering the Bone Hunter button
                {
                    _spriteBatch.Draw(boneHunterUpTexture, boneHunterRectangle, Color.White);
                }

                if (hoverLBK) //Hovering the Legendary Bone Knight button
                {
                    _spriteBatch.Draw(lBKDownTexture, lBKRectangle, Color.White);
                }
                else //Not hovering the Legendary Bone Knight button
                {
                    _spriteBatch.Draw(lBKUpTexture, lBKRectangle, Color.White);
                }

                if (hoverMustDie) //Hovering the Must Die button
                {
                    _spriteBatch.Draw(mustDieDownTexture, mustDieRectangle, Color.White);
                }
                else //Not hovering the Must Die button
                {
                    _spriteBatch.Draw(mustDieUpTexture, mustDieRectangle, Color.White);
                }

            }
            else if (currentScreen == Screen.Controls) //Controls screen
            {

            }
            else if (currentScreen == Screen.Information) //Information screen
            {

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
            }
            else if (currentScreen == Screen.LoseScreen) //Lose screen
            {
                GraphicsDevice.Clear(Color.Black);

                player.Draw(_spriteBatch);
            }

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}