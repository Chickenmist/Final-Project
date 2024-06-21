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
        SlashTwo,
        Jump,
        Hurt,
        HurtFlying,
        FlyingIdle,
        HorizontalBeam,
        VerticalBeam,

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
        MusicScreen,
        Naming,
        Inspriation,
        FightScreen,
        WinScreen,
        LoseScreen
    }

    public class Game1 : Game
    {
        Player player;
        Boss boss;
        Beam beam;

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
        Texture2D playerDeathTexture;
        Texture2D playerJumpTexture;
        //

        //Boss Tetxures
        List<Texture2D> bossTextures = new List<Texture2D>();

        Texture2D bossPhaseOneIdleTexture;
        Texture2D bossSlashOneTexture;
        Texture2D bossSlashTwoTexture;
        Texture2D bossDashTexture;
        Texture2D bossLightingAttackTexture;

        Texture2D bossJumpTexture;
        Texture2D bossHurtTexture;
        Texture2D bossHurtFlyingTexture;
        Texture2D bossDeadTexture;

        Texture2D bossPhaseTwoIdleTexture;
        Texture2D bossHorizontalBeamTexture;
        Texture2D bossVerticalBeamTexture;
        //

        //Beam texture
        Texture2D beamTexture;
        //

        //Background and floor
        Rectangle background;
        Texture2D battleBackgroundTexture;
        Texture2D titleCardTexture;
        Rectangle floor;
        //

        //Button Textures


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

        Rectangle humanRectangle;
        Rectangle boneHunterRectangle;
        Rectangle lBKRectangle;
        Rectangle mustDieRectangle;
        //

        //Variables to check which button the mouse is hovering

        bool hoverHuman;
        bool hoverBoneHunter;
        bool hoverLBK;
        bool hoverMustDie;
        //

        Texture2D rectangleTexture;

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

            difficultySelected = Difficulty.MustDie;

            humanRectangle = new Rectangle(370, 30, 128, 32);
            boneHunterRectangle = new Rectangle(370, 64, 128, 32);
            lBKRectangle = new Rectangle(370, 96, 128, 32);
            mustDieRectangle = new Rectangle(370, 128, 128, 32);
            base.Initialize();
            
            //Player
            playerTextures.Add(playerIdleTexture);
            playerTextures.Add(playerWalkTexture);
            playerTextures.Add(playerRunTexture);
            playerTextures.Add(playerAttackTexture);
            playerTextures.Add(playerJumpTexture);
            playerTextures.Add(playerDeathTexture);
            player = new Player(playerTextures, 50, floor.Y - 95);
            //

            //Boss
            bossTextures.Add(bossPhaseOneIdleTexture);
            bossTextures.Add(bossSlashOneTexture);
            bossTextures.Add(bossDashTexture);
            bossTextures.Add(bossLightingAttackTexture);
            bossTextures.Add(bossSlashTwoTexture);
            bossTextures.Add(bossJumpTexture);
            bossTextures.Add(bossHurtTexture);
            bossTextures.Add(bossHurtFlyingTexture);
            bossTextures.Add(bossPhaseTwoIdleTexture);
            bossTextures.Add(bossHorizontalBeamTexture);
            bossTextures.Add(bossVerticalBeamTexture);
            bossTextures.Add(bossDeadTexture);

            boss = new Boss(bossTextures, 830, floor.Y - 105);
            //

            //Beam
            beam = new Beam(beamTexture, 0, 0);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //Player Sprites
            playerAttackTexture = Content.Load<Texture2D>("Skeleton Attack");
            playerIdleTexture = Content.Load<Texture2D>("Skeleton Idle");
            playerDeathTexture = Content.Load<Texture2D>("Skeleton Dead");
            playerRunTexture = Content.Load<Texture2D>("Skeleton Run");
            playerWalkTexture = Content.Load<Texture2D>("Skeleton Walk");
            playerJumpTexture = Content.Load<Texture2D>("Skeleton Jumping");
            //

            //Boss Sprites
            bossPhaseOneIdleTexture = Content.Load<Texture2D>("Boss Ground Idle");
            bossSlashOneTexture = Content.Load<Texture2D>("Boss Attack 1");
            bossSlashTwoTexture = Content.Load<Texture2D>("Boss Attack 2");
            bossDashTexture = Content.Load<Texture2D>("Boss Dash");
            bossLightingAttackTexture = Content.Load<Texture2D>("Lightning Bolt");
            bossJumpTexture = Content.Load<Texture2D>("Boss Jump");
            bossHurtTexture = Content.Load<Texture2D>("Boss Hurt");
            bossHurtFlyingTexture = Content.Load<Texture2D>("Boss Hurt Flying");
            bossPhaseTwoIdleTexture = Content.Load<Texture2D>("Boss Flying Idle");
            bossHorizontalBeamTexture = Content.Load<Texture2D>("Boss Horizontal Beam");
            bossVerticalBeamTexture = Content.Load<Texture2D>("Boss Vertical Beam");
            bossDeadTexture = Content.Load<Texture2D>("Boss Dead");
            //

            //Beam Sprite
            beamTexture = Content.Load<Texture2D>("Beam Sprite");
            //

            //Background Sprites
            battleBackgroundTexture = Content.Load<Texture2D>("Background");
            titleCardTexture = Content.Load<Texture2D>("Title Card");
            //

            //Button Sprites


            humanUpTexture = Content.Load<Texture2D>("Human Up");
            humanDownTexture = Content.Load<Texture2D>("Human Down");
            boneHunterUpTexture = Content.Load<Texture2D>("Bone Hunter Up");
            boneHunterDownTexture = Content.Load<Texture2D>("Bone Hunter Down");
            lBKUpTexture = Content.Load<Texture2D>("LBK Up");
            lBKDownTexture = Content.Load<Texture2D>("LBK Down");
            mustDieUpTexture = Content.Load<Texture2D>("Must Die Up");
            mustDieDownTexture = Content.Load<Texture2D>("Must Die Down");
            //

            rectangleTexture = Content.Load<Texture2D>("rectangle");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Window.Title = $"{mouseState.X} {mouseState.Y}";

            if (currentScreen == Screen.MainMenu)
            {

            }
            else if (currentScreen == Screen.Difficulty)
            {
                if (humanRectangle.Contains(mouseState.Position))
                {
                    hoverHuman = true;

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        boss.GetDifficulty = 1;
                        player.GetDifficulty = 1;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else
                {
                    hoverHuman = false;
                }
                
                if (boneHunterRectangle.Contains(mouseState.Position))
                {
                    hoverBoneHunter = true;

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        boss.GetDifficulty = 2;
                        player.GetDifficulty = 2;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else
                {
                    hoverBoneHunter = false;
                }
                
                if (lBKRectangle.Contains(mouseState.Position))
                {
                    hoverLBK = true;

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        boss.GetDifficulty = 3;
                        player.GetDifficulty = 3;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else
                {
                    hoverLBK = false;
                }
                
                if (mustDieRectangle.Contains(mouseState.Position))
                {
                    hoverMustDie = true;

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        boss.GetDifficulty = 4;
                        player.GetDifficulty = 4;

                        currentScreen = Screen.FightScreen;
                    }
                }
                else
                {
                    hoverMustDie = false;
                }
            }
            else if (currentScreen == Screen.Controls)
            {

            }
            else if (currentScreen == Screen.Information)
            {

            }
            else if (currentScreen == Screen.MusicScreen)
            {

            }
            else if (currentScreen == Screen.Naming)
            {

            }
            else if (currentScreen == Screen.FightScreen)
            {   
                player.Update(gameTime, keyboardState, mouseState, boss);

                boss.Update(gameTime, player, beam);

                beam.Update(gameTime, boss, player);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (currentScreen == Screen.MainMenu)
            {
                _spriteBatch.Draw(titleCardTexture, background, Color.White);
            }
            else if (currentScreen == Screen.Difficulty)
            {
                _spriteBatch.Draw(titleCardTexture, background, Color.White);

                if (hoverHuman)
                {
                    _spriteBatch.Draw(humanDownTexture, humanRectangle, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(humanUpTexture, humanRectangle, Color.White);
                }

                if (hoverBoneHunter)
                {
                    _spriteBatch.Draw(boneHunterDownTexture, boneHunterRectangle, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(boneHunterUpTexture, boneHunterRectangle, Color.White);
                }

                if (hoverLBK)
                {
                    _spriteBatch.Draw(lBKDownTexture, lBKRectangle, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(lBKUpTexture, lBKRectangle, Color.White);
                }

                if (hoverMustDie)
                {
                    _spriteBatch.Draw(mustDieDownTexture, mustDieRectangle, Color.White);
                }
                else
                {
                    _spriteBatch.Draw(mustDieUpTexture, mustDieRectangle, Color.White);
                }

            }
            else if (currentScreen == Screen.Controls)
            {

            }
            else if (currentScreen == Screen.Information)
            {

            }
            else if (currentScreen == Screen.MusicScreen)
            {

            }
            else if (currentScreen == Screen.Naming)
            {

            }
            else if (currentScreen == Screen.Inspriation)
            {

            }
            else if (currentScreen == Screen.FightScreen)
            {
                _spriteBatch.Draw(battleBackgroundTexture, background, Color.White);

                player.Draw(_spriteBatch);
                _spriteBatch.Draw(rectangleTexture, player.playerHurtbox, new Color(Color.Black, 0.5f));
                _spriteBatch.Draw(rectangleTexture, player.playerHitbox, new Color(Color.Red, 0.5f));

                boss.Draw(_spriteBatch);
                _spriteBatch.Draw(rectangleTexture, boss.bossHurtbox, new Color(Color.Black, 0.5f));
                _spriteBatch.Draw(rectangleTexture, boss.bossHitbox, new Color(Color.Red, 0.5f));

                beam.Draw(_spriteBatch);
            }
            else if (currentScreen == Screen.WinScreen)
            {

            }
            else if (currentScreen == Screen.LoseScreen)
            {
                
            }

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}