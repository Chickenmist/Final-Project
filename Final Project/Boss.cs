using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    internal class Boss
    {
        Game1 game;

        private List<Texture2D> _textures;
        private Rectangle _location;
        private Vector2 _speed;

        public Rectangle bossHitbox;

        private Random _random = new Random();
        private int _bossAction;

        BossState _state;

        private int _health;

        private bool _active;

        private bool _facingLeft = true;
        private int _frame = 0;
        private Rectangle _spriteFrame;

        private float _coolDown;

        public Boss(List<Texture2D> textures, int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 119, 85);
            _speed = new Vector2();
        }

        public void Update(GameTime gameTime)
        {
            if (game.bossHealth > 50) //Phase One
            {
                if (_active == false && _coolDown == 0)
                {
                    _bossAction = _random.Next(1, 4);

                    if (_bossAction == 1) //Slash attack
                    {
                        _speed.X = 4;
                        _state = BossState.SlashOne;
                    }
                    else if (_bossAction == 2) //Dash
                    {
                        _speed.X = 9;
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
            else if (game.bossHealth <= 50) //Phase Two
            {

            }
            else if (game.bossHealth == 0) //Dead
            {

            }

            if (_coolDown <= 0)
            {
                _active = false;
                _coolDown = 0;
            }

            Move();

            AnimateFrame();
        }


        public void Move()
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;
        }

        private void AnimateFrame()
        {
            if (_state == BossState.GroundIdle) //Phase one idle (grounded idle)
            {

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
            else if (_state == BossState.HorizontalBeam) // Horizontal beam
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
