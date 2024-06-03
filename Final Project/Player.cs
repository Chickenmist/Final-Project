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
        private float _spriteSeconds;
        private float _attackSeconds;
        private float _attackCoolDown;

        private Rectangle _spriteFrame;
        private float _frame;

        private float _health;

        public Rectangle playerHitbox; //damage dealing area
        public Rectangle playerHurtbox; //damageable area

        PlayerState _state;

        private bool _facingLeft;
        private bool _facingRight = true;
        private bool _jumping;
        private bool _falling;
        private bool _attacking = false;

        public Player(List<Texture2D> textures, int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 100, 95);
            _speed = new Vector2();
            _state = PlayerState.Idle;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            if (_state != PlayerState.Attack || _state != PlayerState.Dead)
            {
                _state = PlayerState.Idle;
            }

            //Jumping
            if (keyboardState.IsKeyDown(Keys.Space))
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
            //

            //Attacking
            if (mouseState.LeftButton == ButtonState.Pressed && _attacking == false && _attackCoolDown == 0) //Attack started
            {
                _state = PlayerState.Attack;
                _frame = 0;
                _spriteSeconds = 0;
                _attacking = true;
                _attackCoolDown = 1.5f;
            }

            if (_attacking == true) //Attacking
            {
                _state = PlayerState.Attack;
                
                _attackSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_attackSeconds > 0.4)
                {
                    _attacking = false;
                    _attackSeconds = 0;
                }
            }

            if (_attackCoolDown > 0)
            {
                _attackCoolDown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (_attackCoolDown <= 0)
            {
                _attackCoolDown = 0;
            }
            //

            //Movement
            if (keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running left
            {
                _facingLeft = true;
                _facingRight = false;

                HSpeed = -8.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Run;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.LeftShift)) //Running right
            {
                _facingRight = true;
                _facingLeft = false;

                HSpeed = 8.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Run;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.A)) //Walking left
            {
                _facingLeft = true;
                _facingRight = false;

                HSpeed = -4.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Walk;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D)) //Walking right
            {
                _facingRight = true;
                _facingLeft = false;

                HSpeed = 4.5f;

                if (_state != PlayerState.Attack && _state != PlayerState.Jump && _state != PlayerState.Dead)
                {
                    _state = PlayerState.Walk;
                }
            }
            
            if (_state == PlayerState.Idle)
            {
                HSpeed = 0;
            }
            
            Move();
            GenerateBoxes();
            //

            //Animation
            _spriteSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            AnimateFrame();
            //
        }

        private void AnimateFrame()
        {
            if (_state == PlayerState.Walk) //Walk cycle
            {
                if (_spriteSeconds >= 0.08)
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
                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Idle) //Idle cycle
            {
                if (_spriteSeconds >= 0.1)
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
                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Jump) //Jump frame
            {
                _spriteFrame = new Rectangle(45, 62, 80, 75);
            }
            else if (_state == PlayerState.Run) //Run cycle
            {
                if (_spriteSeconds >= 0.07)
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
                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Attack) //Attack cycle
            {
                if (_spriteSeconds >= 0.1)
                {
                    if (_frame == 0)
                    {
                        _spriteFrame = new Rectangle(25, 53, 80, 75);

                        _frame++;
                    }
                    else if (_frame == 1)
                    {
                        _spriteFrame = new Rectangle(159, 53, 80, 75);

                        _frame++;
                    }
                    else if (_frame == 2)
                    {
                        _spriteFrame = new Rectangle(296, 53, 80, 75);
                        _frame++;
                    }
                    else if (_frame == 3)
                    {
                        _spriteFrame = new Rectangle(424, 53, 80, 75);
                        _frame = 0;
                    }
                    _spriteSeconds = 0;
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

            if (_facingLeft)
            {
                if (_location.Left < -40)
                {
                    _location.X = -40;
                }
                else if (_location.Right > 960)
                {
                    _location.X = 960 - _location.Width;
                }
            }
            else if (_facingRight)
            {
                if (_location.Left < 0)
                {
                    _location.X = 0;
                }
                else if (_location.Right > 1000)
                {
                    _location.X = 1000 - _location.Width;
                }
            }
        }
        private void GenerateBoxes()
        {
            //Hurtbox
            if (_state != PlayerState.Attack)
            {
                if (_facingLeft)
                {
                    playerHurtbox = new Rectangle(_location.X + 40, _location.Y, _location.Width - 40, _location.Height);
                }
                else if (_facingRight)
                {
                    playerHurtbox = new Rectangle(_location.X, _location.Y, _location.Width - 40, _location.Height);
                }
            }
            else if (_state == PlayerState.Attack && _facingRight)
            {
                playerHurtbox = new Rectangle(_location.X, _location.Y, _location.Width - 40, _location.Height);
            }
            else if (_state == PlayerState.Attack && _facingLeft)
            {
                playerHurtbox = new Rectangle(_location.X + 40, _location.Y, _location.Width - 40, _location.Height);
            }
            //

            //Hitbox
            if (_state != PlayerState.Attack)
            {
                playerHitbox = Rectangle.Empty;
            }
            else if (_state == PlayerState.Attack && _facingRight)
            {
                playerHitbox = new Rectangle(playerHurtbox.Right, _location.Y, 40, _location.Height);
            }
            else if (_state == PlayerState.Attack && _facingLeft)
            {
                playerHitbox = new Rectangle(playerHurtbox.Left - 40, _location.Y, 40, _location.Height);
            }
            //
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
