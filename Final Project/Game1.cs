using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        Texture2D skyTexture;
        Texture2D hillsAndTreesTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            base.Initialize();

            playerTextures.Add(playerIdleTexture);
            playerTextures.Add(playerWalkTexture);
            playerTextures.Add(playerRunTexture);
            playerTextures.Add(playerAttackTexture);
            playerTextures.Add(playerRunAttackTexture);
            playerTextures.Add(playerJumpTexture);
            playerTextures.Add(playerDeathTexture);
            player = new Player(playerTextures, 50, 50);
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

            //Background Sprites
            

        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            player.Update(gameTime, keyboardState, mouseState);

            player.Move();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            player.Draw(_spriteBatch);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}