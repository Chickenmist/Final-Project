using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    internal class Player
    {

        private List<Texture2D> _textures;
        private Rectangle _location;
        private Vector2 _speed;
        private float _seconds;

        private Rectangle _spriteFrame;
        private float _frame;

        PlayerState _state;

        public Player(List<Texture2D> textures, int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 53, 58);
            _speed = new Vector2();
            _state = PlayerState.Idle;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _state = PlayerState.Idle;
            
            if (keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running left
            {
                
            }
            else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running right
            {
                
            }
            else if (keyboardState.IsKeyDown(Keys.A)) //Walking left
            {

            }
            else if (keyboardState.IsKeyDown(Keys.D)) //Walking right
            {

            }

            if (keyboardState.IsKeyUp(Keys.None))
            {
                
            }

            _seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_state == PlayerState.Idle)
            {
                if (_seconds >= 0.4)
                {
                    if (_frame == 0)
                    {
                        _spriteFrame = new Rectangle();
                    }
                    
                    _frame++;
                }
            }

        }

        public PlayerState State 
        { 
            get { return _state; } 
            set { _state = value; }
        }

        public float HSpeed
        {
            get { return _speed.X; }
            set { _speed.X = value; }
        }

        public float VSpeed
        {
            get { return _speed.Y; }
            set { _speed.Y = value; }
        }

        private void Move()
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White);
        }
    }
}
