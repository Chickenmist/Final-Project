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
            _location = new Rectangle(x, y, 80, 75);
            _speed = new Vector2();
            _state = PlayerState.Idle;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _state = PlayerState.Idle;

            _seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running left
            {
                
            }
            else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running right
            {
                
            }
            else if (keyboardState.IsKeyDown(Keys.A)) //Walking left. Widest frame is 59
            {
                _state = PlayerState.WalkLeft;

                if (_state == PlayerState.WalkLeft)
                {
                    if (_seconds >= 0.1)
                    {
                        if (_frame == 0)
                        {
                            _spriteFrame = new Rectangle(46, 56, 80, 65);

                            _frame++;
                        }
                        else if (_frame == 1)
                        {
                            _spriteFrame = new Rectangle(142, 56, 80, 65);

                            _frame++;
                        }
                        else if (_frame == 2)
                        {
                            _spriteFrame = new Rectangle(294, 56, 80, 65);
                            _frame++;
                        }
                        else if (_frame == 3)
                        {
                            _spriteFrame = new Rectangle(422, 56, 80, 65);
                            _frame++;
                        }
                        else if (_frame == 4)
                        {
                            _spriteFrame = new Rectangle(550, 56, 80, 65);
                            _frame++;
                        }
                        else if (_frame == 5)
                        {
                            _spriteFrame = new Rectangle(678, 56, 80, 58);
                            _frame++;
                        }
                        else if (_frame == 6)
                        {
                            _spriteFrame = new Rectangle(806, 56, 80, 58);
                            _frame = 0;
                        }
                        _seconds = 0;
                    }
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D)) //Walking right
            {

            }
            
            if (keyboardState.IsKeyUp(Keys.None))
            {
                if (_state == PlayerState.Idle)
                {
                    if (_seconds >= 0.1)
                    {
                        if (_frame == 0)
                        {
                            _spriteFrame = new Rectangle(38, 70, 80, 58);

                            _frame++;
                        }
                        else if (_frame == 1)
                        {
                            _spriteFrame = new Rectangle(166, 70, 80, 58);

                            _frame++;
                        }
                        else if (_frame == 2)
                        {
                            _spriteFrame = new Rectangle(294, 70, 80, 58);
                            _frame++;
                        }
                        else if (_frame == 3)
                        {
                            _spriteFrame = new Rectangle(422, 70, 80, 58);
                            _frame++;
                        }
                        else if (_frame == 4)
                        {
                            _spriteFrame = new Rectangle(550, 70, 80, 58);
                            _frame++;
                        }
                        else if (_frame == 5)
                        {
                            _spriteFrame = new Rectangle(678, 70, 80, 58);
                            _frame++;
                        }
                        else if (_frame == 6)
                        {
                            _spriteFrame = new Rectangle(806, 70, 80, 58);
                            _frame = 0;
                        }
                        
                        _seconds = 0;
                    }
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
            if (_state == PlayerState.RunLeft || _state == PlayerState.WalkLeft)
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White, 0f, new Vector2(0,0), SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White);
            }
        }
    }
}
