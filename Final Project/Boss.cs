using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Boss
    {
        private List<Texture2D> _textures;
        private Rectangle _location;
        private Vector2 _speed;

        public Rectangle bossHitbox; //damage dealing area
        public Rectangle bossHurtbox; //damageable area

        private Random _random = new Random();
        private int _bossAction;

        BossState _state;

        private int _difficulty; //1 = Human  2 = Bone Hunter  3 = LBK  4 = Must Die

        private int _health;

        private bool _active;
        private bool _falling = false;
        public bool phaseTwoTransition;
        private bool _dead;

        private bool _facingLeft = true; 
        private int _frame = 0;
        private float _frameTime = 0;
        private Rectangle _spriteFrame;

        private float _coolDown;

        public Boss(List<Texture2D> textures,int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 139, 105);
            _speed = new Vector2();
            _health = 100;
            _coolDown = 3;
        }

        public void Update(GameTime gameTime, Player player)
        {
            _bossAction = 0;

            if (_state != BossState.Dash && player.playerHurtbox.X < _location.X)
            {
                _facingLeft = true;
            }
            else if (_state != BossState.Dash && player.playerHurtbox.X > _location.X)
            {
                _facingLeft = false;
            }

            if (_health > 50) //Phase One
            {
                if (_active == false && _coolDown == 0)
                {
                    //_bossAction = _random.Next(1, 4);

                    _frame = 0;

                    if (_bossAction == 1) //Slash attack
                    {
                        _state = BossState.SlashOne;
                    }
                    else if (_bossAction == 2) //Dash
                    {
                        _state = BossState.Dash;
                    }
                    else if (_bossAction == 3) //Lighting blast
                    {
                        _state = BossState.LightningBolt;
                    }

                    _active = true;
                }

                if (_state == BossState.SlashOne || _state == BossState.SlashTwo)
                {
                    if (_facingLeft)
                    {
                        if(_location.X > player.playerHurtbox.X + 10)
                        {
                            _speed.X = -2;
                        }
                    }
                    else
                    {
                        if (_location.X < player.playerHurtbox.X - 10)
                        {
                            _speed.X = 2;
                        }
                    }
                }
                else if (_state == BossState.Dash)
                {
                    if (_facingLeft)
                    {
                        _speed.X = -6;
                    }
                    else
                    {
                        _speed.X = 6;
                    }
                }
                else if (_state == BossState.LightningBolt || _state == BossState.GroundIdle)
                {
                    _speed.X = 0;
                }
                else if (_state == BossState.Hurt)
                {
                    _speed.X = 0;
                }
                else if (_state == BossState.Jump)
                {
                    if (_facingLeft)
                    {
                        _speed.X = 2;
                    }
                    else
                    {
                        _speed.X = -2;
                    }

                    if (_location.Y > 200 && _falling == false)
                    {
                        _speed.Y = -2;
                    }
                    else if (_location.Y <= 200 && _falling == false)
                    {
                        _falling = true;
                        _speed.Y = 2;
                    }
                    else if (_location.Bottom >= 490 && _falling)
                    {
                        _falling = false;
                        _speed.Y = 0;
                        _location.Y = 490 - bossHurtbox.Height;
                        StartCooldown();
                    }
                }
                else if (_state == BossState.GroundIdle)
                {
                    _speed.Y = 0;
                    _speed.X = 0;
                }

            }
            else if (_health == 50 && phaseTwoTransition) //Phase two transition
            {
                if (_location.Y > 250)
                {
                    _speed.Y = -3;
                    _state = BossState.Jump;
                    _frame = 2;
                }
                else if (_location.Y <= 250)
                {
                    _speed.Y = 0;
                    _state = BossState.FlyingIdle;
                    phaseTwoTransition = false;
                }
            }
            else if (_health <= 50 && phaseTwoTransition == false) //Phase Two
            {

            }
            else if (_health == 0) //Dead
            {

            }

            if (_coolDown > 0)
            {
                if (_health > 50)
                {
                    _state = BossState.GroundIdle;
                }
                else if (_health <= 50)
                {
                    _state = BossState.FlyingIdle;
                }
                
                _coolDown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (_coolDown <= 0)
            {
                _coolDown = 0;
            }

            //if(player.playerHitbox.Intersects(bossHurtbox))
            //{
            //    TakeDamage();
            //}

            Move();
            GenerateBoxes();

            _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            AnimateFrame();
        }


        private void Move()
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;

            if (_facingLeft)
            {
                if (_location.Left < -73) //Stops it from going off screen to the left when facing left
                {
                    _location.X = -73;

                    if (_state == BossState.Dash) //Ends the dash attack
                    {
                        StartCooldown();
                        _active = false;
                    }
                }
                else if (_location.Right > 960)
                {
                    _location.X = 960 - _location.Width;
                }
            }
            else
            {
                if (_location.Left < 0) //Stops it from going off screen to the left when facing right
                {
                    _location.X = 0;
                }
                else if (_location.Right > 1033) //Stops it from going off screen to the right when facing right
                { 
                    _location.X = 887;

                    if (_state == BossState.Dash) //Ends the dash attack
                    {
                        StartCooldown();
                        _active = false;
                    }
                }
            }
        }

        public void StartCooldown()
        {
            if (_difficulty == 1) //Human
            {
                _coolDown = 4;
            }
            else if (_difficulty == 2) //Bone Hunter
            {
                _coolDown = 3;
            }
            else if (_difficulty == 3 || _difficulty == 4) //LBK and Must Die
            {
                _coolDown = 2;
            }
        }

        public int GetDifficulty
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }

        private void AnimateFrame()
        {
            if (_state == BossState.GroundIdle) //Phase one idle (grounded idle)
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(32, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(159, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(284, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(411, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(539, 43, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(665, 43, 119, 85);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(795, 43, 119, 85);
                }

                if (_frameTime >= 0.1)
                {
                    if (_frame >= 6)
                    {
                        _frame = 0;
                    }
                    else
                    {
                        _frame++;
                    }

                    _frameTime = 0;
                }
            }
            else if(_state == BossState.SlashOne) //Slash one
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(24, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(151, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(296, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(425, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(551, 43, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(681, 43, 119, 85);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(805, 43, 119, 85);
                }
                else if (_frame == 7)
                {
                    _spriteFrame = new Rectangle(933, 43, 119, 85);
                }
                else if (_frame == 8)
                {
                    _spriteFrame = new Rectangle(1062, 43, 119, 85);
                }
                else if (_frame == 9)
                {
                    _spriteFrame = new Rectangle(1189, 43, 119, 85);
                    _frame = 0;
                    _state = BossState.SlashTwo;
                }

                if (_frameTime >= 0.1)
                {
                    if (_frame >= 9)
                    {
                        _frame = 0;
                    }
                    else
                    {
                        _frame++;
                    }
                    _frameTime = 0;
                }
            }
            else if(_state == BossState.SlashTwo) //Slash two
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(21, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(150, 43, 106, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(256, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(400, 43, 119, 85);
                }

                if (_frameTime >= 0.15)
                {
                    if (_frame >= 3)
                    {
                        _frame = 0;
                        StartCooldown();
                        _active = false;
                    }
                    else
                    {
                        _frame++;
                    }
                    _frameTime = 0;
                }
            }
            else if(_state == BossState.Dash) //Dash
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(42, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(168, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(290, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(424, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(560, 43, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(678, 43, 119, 85);
                }
                else if ( _frame == 6)
                {
                    _spriteFrame = new Rectangle(804, 43, 119, 85);
                }
                else if (_frame == 7)
                {
                    _spriteFrame = new Rectangle(936, 43, 119, 85);
                }

                if (_frameTime >= 0.06)
                {
                    if (_frame >= 7)
                    {
                        _frame = 0;
                    }
                    else
                    {
                        _frame++;
                    }
                    _frameTime = 0;
                }
            }
            else if(_state == BossState.LightningBolt) //Lightning Attack
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(11, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(133, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(267, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(395, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(521, 43, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(649, 43, 119, 85);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(777, 43, 119, 85);
                }
                else if (_frame == 7)
                {
                    _spriteFrame = new Rectangle(906, 43, 119, 85);
                }
                else if (_frame == 8)
                {
                    _spriteFrame = new Rectangle(1036, 43, 119, 85);
                }
                else if (_frame == 9)
                {
                    _spriteFrame = new Rectangle(1164, 43, 119, 85);
                }
                else if (_frame == 10)
                {
                    _spriteFrame = new Rectangle(1292, 43, 119, 85);
                }
                else if (_frame == 11)
                {
                    _spriteFrame = new Rectangle(1421, 43, 119, 85);
                }

                if (_frameTime >= 0.06)
                {
                    if (_frame >= 11)
                    {
                        _frame = 0;
                        StartCooldown();
                        _active = false;
                    }
                    else
                    {
                        _frame++;
                    }
                    _frameTime = 0;
                }
            }
            else if (_state == BossState.Jump) //Jump
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(49, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(179, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(306, 37, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(440, 33, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(561, 26, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(691, 43, 119, 85);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(819, 43, 119, 85);
                }
                else if (_frame == 7)
                {
                    _spriteFrame = new Rectangle(955, 43, 119, 85);
                }

                if (_frameTime >= 0.2)
                {
                    if (_frame >= 7)
                    {
                        _frame = 0;
                        StartCooldown();
                        _active = false;
                    }
                    else
                    {
                        _frame++;
                    }

                    _frameTime = 0;
                }
            }
            else if (_state == BossState.Hurt)
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(40, 43, 119, 85);
                }
                else if ( _frame == 1)
                {
                    _spriteFrame = new Rectangle(168, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(296, 43, 119, 85);
                }

                if (_frameTime >= 0.06)
                {
                    if (_frame >= 2)
                    {
                        _frame = 0;
                        _state = BossState.Jump;
                    }
                    else
                    {
                        _frame++;
                    }
                    _frameTime = 0;
                }
            }
            else if (_state == BossState.FlyingIdle) //Phase two idle (flying idle)
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(26, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(154, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(282, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(411, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(538, 43, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(666, 43, 119, 85);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(794, 43, 119, 85);
                }

                if (_frameTime >= 0.1)
                {
                    if (_frame >= 6)
                    {
                        _frame = 0;
                    }
                    else
                    {
                        _frame++;
                    }

                    _frameTime = 0;
                }
            }
            else if (_state == BossState.HorizontalBeam) //Horizontal beam
            {

            }
            else if (_state == BossState.VerticalBeam) //Vertical beam
            {

            }
        }

        private void GenerateBoxes()
        {
            if (_facingLeft)
            {
                bossHurtbox = new Rectangle(_location.X + 73, _location.Y + 22, _location.Width - 73, _location.Height -22); //Hurtbox size never needs to change
                bossHitbox = Rectangle.Empty;
                
                if (_state == BossState.GroundIdle || _state == BossState.FlyingIdle)
                {
                    bossHitbox = Rectangle.Empty;
                }
                else if (_state == BossState.SlashOne || _state == BossState.SlashTwo)
                {
                    bossHitbox = new Rectangle(bossHurtbox.Left - 110, bossHurtbox.Y, 110, bossHurtbox.Height);
                }
                else if (_state == BossState.LightningBolt)
                {
                    if (_frame <= 3)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.X, bossHurtbox.Y, bossHurtbox.Width, bossHurtbox.Height);
                    }
                    else if (_frame == 4)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left - 88, bossHurtbox.Y, 88, bossHurtbox.Height);
                    }
                    else if (_frame >= 5 && _frame <= 7)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left - 118, bossHurtbox.Y, 118, bossHurtbox.Height);
                    }
                    else if (_frame == 8 || _frame == 9)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left -115, bossHurtbox.Y, 115, bossHurtbox.Height);
                    }
                    else if (_frame == 10) 
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left - 112, bossHurtbox.Y, 112, bossHurtbox.Height);
                    }
                    else if (_frame == 11)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left - 107, bossHurtbox.Y, 107, bossHurtbox.Height);
                    }   
                }
            }
            else
            {
                bossHurtbox = new Rectangle(_location.X, _location.Y + 22, _location.Width - 73, _location.Height - 22); //Hurtbox size never needs to change

                if (_state == BossState.GroundIdle || _state == BossState.FlyingIdle)
                {
                    bossHitbox = Rectangle.Empty;
                }
                else if (_state == BossState.SlashOne || _state == BossState.SlashTwo)
                {
                    bossHitbox = new Rectangle(_location.X, bossHurtbox.Y, 110, bossHurtbox.Height);
                }
                else if (_state == BossState.LightningBolt)
                {
                    if (_frame <= 3)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.X, bossHurtbox.Y, bossHurtbox.Width, bossHurtbox.Height);
                    }
                    else if (_frame == 4)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left, bossHurtbox.Y, 88, bossHurtbox.Height);
                    }
                    else if (_frame >= 5 && _frame <= 7)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left, bossHurtbox.Y, 118, bossHurtbox.Height);
                    }
                    else if (_frame == 8 || _frame == 9)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left, bossHurtbox.Y, 115, bossHurtbox.Height);
                    }
                    else if (_frame == 10)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left, bossHurtbox.Y, 112, bossHurtbox.Height);
                    }
                    else if (_frame == 11)
                    {
                        bossHitbox = new Rectangle(bossHurtbox.Left, bossHurtbox.Y, 107, bossHurtbox.Height);
                    }
                }
            }
        }

        public void TakeDamage()
        {
            if (_difficulty == 1) //Human
            {
                _health -= 10;
            }
            else if (_difficulty == 2) //Bone Hunter
            {
                _health -= 5;
            }
            else if (_difficulty == 3 || _difficulty == 4) //LBK and Must Die
            {
                _health -= 2;
            }

            if (_health > 50)
            {
                _state = BossState.Hurt;
            }
            else if (_health <= 50)
            {
                _state = BossState.HurtFlying;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_facingLeft) //Facing left
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
            }
            else //Facing right
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White);
            }
        }
    }
}
