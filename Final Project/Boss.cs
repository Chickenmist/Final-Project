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

                    }

                    _active = true;
                }
                else if(_active == false && _coolDown > 0) 
                {
                    _bossAction = 0;
                    _speed.X = 0;
                }

            }
            else if (_health <= 50) //Phase Two
            {

            }
            else if (_health == 0) //Dead
            {

            }

            if (_coolDown <= 0)
            {
                _active = false;
                _coolDown = 0;
            }

            Move();

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
                if (_frameTime >= 0.1)
                {
                    if(_frame == 0)
                    {
                        _spriteFrame = new Rectangle(39, 43, 119, 85);
                        _frame++;
                    }
                    else if(_frame == 1)
                    {
                        _spriteFrame = new Rectangle(166, 43, 119, 85);
                        _frame++;
                    }
                    else if (_frame == 2)
                    {
                        _spriteFrame = new Rectangle(291, 43, 119, 85);
                        _frame++;
                    }
                    else if (_frame == 3)
                    {
                        _spriteFrame = new Rectangle(418, 43, 119, 85);
                        _frame++;
                    }
                    else if (_frame == 4)
                    {
                        _spriteFrame = new Rectangle(546, 43, 119, 85);
                        _frame++;
                    }
                    else if (_frame == 5)
                    {
                        _spriteFrame = new Rectangle(672, 43, 119, 85);
                        _frame++;
                    }
                    else if (_frame == 6)
                    {
                        _spriteFrame = new Rectangle(802, 43, 119, 85);
                        _frame = 0;
                    }

                    _frameTime = 0;
                }
            }
            else if(_state == BossState.SlashOne) //Slash one
            {
                
            }
            else if(_state == BossState.SlashTwo) //slash two
            {

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
            else if (_state == BossState.FlyingIdle) //Phase two idle (flying idle)
            {

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
