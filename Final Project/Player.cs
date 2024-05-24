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

        private bool _facingLeft;
        private bool _facingRight;

        public Player(List<Texture2D> textures, int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 80, 75);
            _speed = new Vector2();
            _state = PlayerState.Idle;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            if (_state != PlayerState.Attack || _state != PlayerState.DashAttack || _state != PlayerState.Jump || _state != PlayerState.Dead)
            {
                _state = PlayerState.Idle;
            }

            _seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _state = PlayerState.Jump;

                _spriteFrame = new Rectangle(0, 53, 80, 75);



                VSpeed += 2;
            }


            if (keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running left
            {
                _facingLeft = true;
                _facingRight = false;

                _state = PlayerState.Run;
            }
            else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running right
            {
                _facingRight = true;
                _facingLeft = false;

                _state = PlayerState.Run;
            }
            else if (keyboardState.IsKeyDown(Keys.A)) //Walking left
            {
                _facingLeft = true;
                _facingRight = false;

                if (_state != PlayerState.Attack || _state != PlayerState.DashAttack || _state != PlayerState.Jump || _state != PlayerState.Dead)
                {
                    _state = PlayerState.Walk;
                }

                if (_state == PlayerState.Walk)
                {
                    if (_seconds >= 0.1)
                    {
                        if (_frame == 0)
                        {
                            _spriteFrame = new Rectangle(46, 53, 80, 75);

                            _frame++;
                        }
                        else if (_frame == 1)
                        {
                            _spriteFrame = new Rectangle(178, 53, 80, 75);

                            _frame++;
                        }
                        else if (_frame == 2)
                        {
                            _spriteFrame = new Rectangle(300, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 3)
                        {
                            _spriteFrame = new Rectangle(425, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 4)
                        {
                            _spriteFrame = new Rectangle(555, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 5)
                        {
                            _spriteFrame = new Rectangle(689, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 6)
                        {
                            _spriteFrame = new Rectangle(814, 53, 80, 75);
                            _frame = 0;
                        }
                        _seconds = 0;
                    }
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D)) //Walking right
            {
                _facingRight = true;
                _facingLeft = false;

                if (_state != PlayerState.Attack || _state != PlayerState.DashAttack || _state != PlayerState.Jump || _state != PlayerState.Dead)
                {
                    _state = PlayerState.Walk;
                }

                if (_state == PlayerState.Walk)
                {
                    if (_seconds >= 0.1)
                    {
                        if (_frame == 0)
                        {
                            _spriteFrame = new Rectangle(46, 53, 80, 75);

                            _frame++;
                        }
                        else if (_frame == 1)
                        {
                            _spriteFrame = new Rectangle(178, 53, 80, 75);

                            _frame++;
                        }
                        else if (_frame == 2)
                        {
                            _spriteFrame = new Rectangle(300, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 3)
                        {
                            _spriteFrame = new Rectangle(425, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 4)
                        {
                            _spriteFrame = new Rectangle(555, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 5)
                        {
                            _spriteFrame = new Rectangle(689, 53, 80, 75);
                            _frame++;
                        }
                        else if (_frame == 6)
                        {
                            _spriteFrame = new Rectangle(814, 53, 80, 75);
                            _frame = 0;
                        }
                        _seconds = 0;
                    }
                }
            }
            
            if (_state == PlayerState.Idle)
            {
                if (_seconds >= 0.1)
                {
                    if (_frame == 0)
                    {
                        _spriteFrame = new Rectangle(38, 53, 80, 75);

                        _frame++;
                    }
                    else if (_frame == 1)
                    {
                        _spriteFrame = new Rectangle(166, 53, 80, 75);

                        _frame++;
                    }
                    else if (_frame == 2)
                    {
                        _spriteFrame = new Rectangle(294, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 3)
                    {
                        _spriteFrame = new Rectangle(422, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 4)
                    {
                        _spriteFrame = new Rectangle(550, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 5)
                    {
                        _spriteFrame = new Rectangle(678, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 6)
                    {
                        _spriteFrame = new Rectangle(806, 53, 80, 75);
                        _frame = 0;
                    }

                    _seconds = 0;
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

        public void Move()
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            if (_facingLeft)
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White, 0f, new Vector2(0,0), SpriteEffects.FlipHorizontally, 0f);
            }
            else if ( _facingRight)
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White);
            }
        }
    }
}
