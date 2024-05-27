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
        DashAttack,
        Jump,
        Dead
    }

    public class Game1 : Game
    {
        Player player;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;
        MouseState mouseState;
        PlayerState playerState;

        List<Texture2D> playerTextures = new List<Texture2D>();
        
        Texture2D playerIdleTexture;
        Texture2D playerWalkTexture;
        Texture2D playerRunTexture;
        Texture2D playerRunAttackTexture;
        Texture2D playerAttackTexture;
        Texture2D playerDeathTexture;
        Texture2D playerJumpTexture;

        Rectangle background;

        Texture2D backgroundTexture;

        Rectangle floor;

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

            base.Initialize();

            playerTextures.Add(playerIdleTexture);
            playerTextures.Add(playerWalkTexture);
            playerTextures.Add(playerRunTexture);
            playerTextures.Add(playerAttackTexture);
            playerTextures.Add(playerRunAttackTexture);
            playerTextures.Add(playerJumpTexture);
            playerTextures.Add(playerDeathTexture);
            player = new Player(playerTextures, 50, floor.Y - 75);
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
            playerRunAttackTexture = Content.Load<Texture2D>("Skeleton Run Attack");
            playerWalkTexture = Content.Load<Texture2D>("Skeleton Walk");
            playerJumpTexture = Content.Load<Texture2D>("Skeleton Jump");
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

            Window.Title = $"{mouseState.X} {mouseState.Y}" ;

            player.Update(gameTime, keyboardState, mouseState);

            player.Move();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, background, Color.White);

            player.Draw(_spriteBatch);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}