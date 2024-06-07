using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Difficulty difficultySelected;

        Screen currentScreen;

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
        Texture2D bossDeadTexture;

        Texture2D bossPhaseTwoIdleTexture;
        Texture2D bossHorizontalBeamTexture;
        Texture2D bossVerticalBeamTexture;
        //

        //Background and floor
        Rectangle background;
        Texture2D backgroundTexture;
        Rectangle floor;
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

            currentScreen = Screen.FightScreen;

            base.Initialize();
            
            //Player
            playerTextures.Add(playerIdleTexture);
            playerTextures.Add(playerWalkTexture);
            playerTextures.Add(playerRunTexture);
            playerTextures.Add(playerAttackTexture);
            playerTextures.Add(playerJumpTexture);
            playerTextures.Add(playerDeathTexture);
            player = new Player(playerTextures, 50, floor.Y - 75);
            //

            //Boss
            bossTextures.Add(bossPhaseOneIdleTexture);
            bossTextures.Add(bossSlashOneTexture);
            bossTextures.Add(bossDashTexture);
            bossTextures.Add(bossLightingAttackTexture);
            bossTextures.Add(bossSlashTwoTexture);
            bossTextures.Add(bossJumpTexture);
            bossTextures.Add(bossHurtTexture);
            bossTextures.Add(bossPhaseTwoIdleTexture);
            bossTextures.Add(bossHorizontalBeamTexture);
            bossTextures.Add(bossVerticalBeamTexture);
            bossTextures.Add(bossDeadTexture);

            boss = new Boss(bossTextures,  40, 40);
            //
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
            bossPhaseTwoIdleTexture = Content.Load<Texture2D>("Boss Flying");
            bossHorizontalBeamTexture = Content.Load<Texture2D>("Boss Horizontal Beam");
            bossVerticalBeamTexture = Content.Load<Texture2D>("");
            bossDeadTexture = Content.Load<Texture2D>("Boss Dead");
            //

            //Background Sprite
            backgroundTexture = Content.Load<Texture2D>("Background");
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

            player.Update(gameTime, keyboardState, mouseState);

            boss.Update(gameTime, player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (currentScreen == Screen.MainMenu)
            {

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
                _spriteBatch.Draw(backgroundTexture, background, Color.White);

                player.Draw(_spriteBatch);
                //_spriteBatch.Draw(rectangleTexture, player.playerHurtbox, new Color(Color.Black, 0.5f));
                //_spriteBatch.Draw(rectangleTexture, player.playerHitbox, new Color(Color.Red, 0.5f));

                boss.Draw(_spriteBatch);
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