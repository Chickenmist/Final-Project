using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Final_Project
{
    public class Game1 : Game
    {
        Player player;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;

        Texture2D playerAttackTexture;
        Texture2D playerIdleTexture; //53x58
        Texture2D playerDeathTexture;
        Texture2D playerRunTexture;
        Texture2D playerRunAttackTexture;
        Texture2D playerWalkTexture; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            player = new Player(playerIdleTexture, 50, 50);
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            playerAttackTexture = Content.Load<Texture2D>("Skeleton Attack");
            playerIdleTexture = Content.Load<Texture2D>("Skeleton Idle");
            playerDeathTexture = Content.Load<Texture2D>("Skeleton Dead");
            playerRunTexture = Content.Load<Texture2D>("Skeleton Run");
            playerRunAttackTexture = Content.Load<Texture2D>("Skeleton Run Attack");
            playerWalkTexture = Content.Load<Texture2D>("Skeleton Walk");

        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.LeftShift))
            {

            }
            else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.LeftShift))
            {

            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {

            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {

            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {

            }

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