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

        public Rectangle bossHitbox;
        public Rectangle bossHurtbox;

        private Random _random = new Random();
        private int _bossAction;

        BossState _state;

        private int _health;

        private bool _active;

        private bool _facingLeft = true; 
        private int _frame = 0;
        private float _frameTime = 0;
        private Rectangle _spriteFrame;

        private float _coolDown;

        public Boss(List<Texture2D> textures,int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 119, 85);
            _speed = new Vector2();
            _health = 100;
        }

        public void Update(GameTime gameTime, Player player)
        {
            _bossAction = 0;

            if (player.playerHurtbox.X < _location.X)
            {
                _facingLeft = true;
            }
            else if (player.playerHurtbox.X > _location.X)
            {
                _facingLeft = false;
            }

            if (_health > 50) //Phase One
            {
                if (_active == false && _coolDown == 0)
                {
                    //_bossAction = _random.Next(1, 4);

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
                        _speed.X = -4;
                    }
                    else
                    {
                        _speed.X = 4;
                    }
                }
                else if (_state == BossState.Dash)
                {

                }
                else if (_state == BossState.LightningBolt || _state == BossState.GroundIdle)
                {
                    _speed.X = 0;
                }

            }
            else if (_health <= 50) //Phase Two
            {

            }
            else if (_health == 0) //Dead
            {

            }

            //Move();

            if (_coolDown >= 0)
            {
                if (_health > 50)
                {
                    _state = BossState.FlyingIdle;
                }
                else if (_health <= 50)
                {

                }
                
                _coolDown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (_coolDown <= 0)
            {
                _coolDown = 0;
            }

            _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            AnimateFrame();
        }


        private void Move()
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;
        }

        private void AnimateFrame()
        {
            if (_state == BossState.GroundIdle) //Phase one idle (grounded idle)
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(13, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(140, 43, 119, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(265, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(392, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(520, 43, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(646, 43, 119, 85);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(776, 43, 119, 85);
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
                        _coolDown = 2;
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
                
            }
            else if(_state == BossState.LightningBolt) //Lightning Attack
            {

            }
            else if (_state == BossState.Jump) //Jump
            {

            }
            else if (_state == BossState.Hurt)
            {

            }
            else if (_state == BossState.FlyingIdle) //Phase two idle (flying idle)
            {
                _spriteFrame = new Rectangle(10, 43, 119, 85);
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

            }
            else
            {

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
