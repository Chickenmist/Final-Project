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

        private Rectangle _hitbox; //damage dealing area
        private Rectangle _hurtbox; //damageable area

        PlayerState _state;

        private bool _facingLeft;
        private bool _facingRight = true;
        private bool _jumping;
        private bool _falling;

        public Player(List<Texture2D> textures, int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 100, 95);
            _speed = new Vector2();
            _state = PlayerState.Idle;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            _hurtbox = new Rectangle(_location.X, _location.Y, 80, 75);

             _seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
           
            if (_state != PlayerState.Idle)
            {
                _state = PlayerState.Idle;
            }

            if (keyboardState.IsKeyDown(Keys.Space)) //Jump
            {
                HSpeed = 0;

                if (_jumping == false)
                {
                    _state = PlayerState.Jump;

                    _jumping = true;
                }
                else if (_jumping) //Jump is held
                {
                    _state = PlayerState.Jump;

                    if (_location.Y > 230 && _falling == false)
                    {
                        VSpeed = -6;
                    }
                    else if (_location.Y <= 230 && _falling == false || keyboardState.IsKeyUp(Keys.Space) && _falling == false)
                    {
                        _falling = true;
                    }

                    if (_falling == true)
                    {
                        VSpeed = 6;

                        if (_location.Bottom >= 490)
                        {
                            VSpeed = 0;
                            _falling = false;
                            _jumping = false;
                        }
                    }
                }
            }

            if (_jumping == true && keyboardState.IsKeyUp(Keys.Space)) //Jump is not held
            {
                _state = PlayerState.Jump;

                if (_location.Y > 320 && _falling == false)
                {
                    VSpeed = -6;
                }
                else if (_location.Y <= 320 && _falling == false)
                {
                    _falling = true;
                }

                if (_falling == true)
                {
                    VSpeed = 6;

                    if (_location.Bottom >= 490)
                    {
                        VSpeed = 0;
                        _falling = false;
                        _jumping = false;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running left
            {
                _facingLeft = true;
                _facingRight = false;

                HSpeed = -8.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.DashAttack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Run;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running right
            {
                _facingRight = true;
                _facingLeft = false;

                HSpeed = 8.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.DashAttack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Run;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.A)) //Walking left
            {
                _facingLeft = true;
                _facingRight = false;

                HSpeed = -4.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.DashAttack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Walk;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D)) //Walking right
            {
                _facingRight = true;
                _facingLeft = false;

                HSpeed = 4.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.DashAttack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Walk;
                }
            }
            
            if (_state == PlayerState.Idle)
            {
                HSpeed = 0;
            }
            
            Move();
            AnimateFrame();
        }

        private void AnimateFrame()
        {
            if (_state == PlayerState.Walk) //Walk cycle
            {
                if (_seconds >= 0.08)
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
            else if (_state == PlayerState.Idle) //Idle cycle
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
            else if (_state == PlayerState.Jump) //Jump frame
            {
                _spriteFrame = new Rectangle(0, 53, 80, 75);
            }
            else if (_state == PlayerState.Run) //Run cycle
            {
                if (_seconds >= 0.07)
                {
                    if (_frame == 0)
                    {
                        _spriteFrame = new Rectangle(34, 53, 80, 75);

                        _frame++;
                    }
                    else if (_frame == 1)
                    {
                        _spriteFrame = new Rectangle(163, 53, 80, 75);

                        _frame++;
                    }
                    else if (_frame == 2)
                    {
                        _spriteFrame = new Rectangle(282, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 3)
                    {
                        _spriteFrame = new Rectangle(414, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 4)
                    {
                        _spriteFrame = new Rectangle(550, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 5)
                    {
                        _spriteFrame = new Rectangle(676, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 6)
                    {
                        _spriteFrame = new Rectangle(799, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 7)
                    {
                        _spriteFrame = new Rectangle(931, 53, 80, 75);
                        _frame = 0;
                    }
                    _seconds = 0;
                }
            }
            else if (_state == PlayerState.Attack)
            {

            }
        }

        public bool Collide(Rectangle item)
        {
            return _hurtbox.Intersects(item);
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
