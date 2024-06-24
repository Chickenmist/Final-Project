using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        BossState _state;

        private int _difficulty; //1 = Human  2 = Bone Hunter  3 = LBK  4 = Must Die

        private int _health;

        private bool _active;
        private bool _falling = false;
        public bool attacking;
        public bool damaged;
        public bool bossDead;

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
            _coolDown = 1;
            bossDead = false;
            damaged = false;
            attacking = false;
        }

        public void Update(GameTime gameTime, Player player, Projectile projectile, BossHealthBar bossHealthBar)
        {
            if (_health > 0)
            {
                // Faces boss in correct direction
                if (_state != BossState.Dash && player.playerHurtbox.X < _location.X - 73)
                {
                    _facingLeft = true;
                }
                else if (_state != BossState.Dash && player.playerHurtbox.X > _location.X + 73)
                {
                    _facingLeft = false;
                }
                
                //Boss needs a new action
                if (!_active && _coolDown == 0)
                {
                    _frame = 0;

                    _state = (BossState)_random.Next(1, 5);

                    _active = true;
                }

                if (_state == BossState.SlashOne || _state == BossState.SlashTwo) //Slash attack
                {
                    attacking = true;

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
                else if (_state == BossState.Dash) //Dash attack
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
                else if (_state == BossState.LightningBolt) //Lighting bolt
                {
                    attacking = true;

                    _speed.X = 0;
                    _speed.Y = 0;
                }
                else if (_state == BossState.Projectile) //Projectile attack
                {
                    _speed.X = 0;

                    attacking = true;

                    if (_frame == 2)
                    {
                        if (_facingLeft)
                        {
                            projectile.FireLeft();
                        }
                        else
                        {
                            projectile.FireRight();
                        }
                    }
                }
                else if (_state == BossState.Hurt)
                {
                    _speed.X = 0;
                }
                else if (_state == BossState.Jump)
                {
                    if (_facingLeft)
                    {
                        _speed.X = 5;
                    }
                    else
                    {
                        _speed.X = -5;
                    }

                    // Still jumping
                    if (_location.Top > 290 && !_falling)
                    {
                        _speed.Y = -2;
                    }
                    // Reached top of jump, begin falling
                    else if (_location.Top <= 290)
                    {
                        _speed.Y = 2;
                        _falling = true;
                    }
                    else if (_location.Bottom >= 490 && _falling) //Landed
                    {
                        _falling = false;
                        damaged = false;
                        _state = BossState.GroundIdle;
                        _location.Y = 490 - _location.Height;

                        if (!damaged) //If the jump was started after attacking
                        {
                            StartCooldown();
                        }
                    }
                }
                else if (_state == BossState.GroundIdle)
                {
                    _speed.X = 0;
                    _speed.Y = 0;
                }

                if (bossHitbox.Intersects(player.playerHurtbox) || bossHurtbox.Intersects(player.playerHurtbox)) //Damaged Player
                {
                    _frame = 0;
                    _state = BossState.Jump;
                    player.Damaged();
                }

                if (_coolDown > 0 && _state != BossState.Hurt && _state != BossState.Jump) //Makes the boss idle on it's cooldown
                {
                    _state = BossState.GroundIdle;

                    _coolDown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (_coolDown <= 0)
                {
                    _coolDown = 0;
                }
            }
            else if (_health == 0) //Dead
            {
                _speed.X = 0;
                _speed.Y = 0;

                if (_facingLeft)
                {
                    _location = new Rectangle(350, 490 - _location.Height, 139, 105);
                }
                else
                {
                    _location = new Rectangle(380, 490 - _location.Height, 139, 105);
                }
                _state = BossState.Dead;
            }

            bossHealthBar.LoseHealth = ((float)_health / 100) * 690;
            bossHealthBar.Update();

            Move();
            GenerateBoxes();

            _frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            AnimateFrame();
        }


        private void Move() //Handles movement
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
                    }
                }
            }
        }

        public void StartCooldown() //Decides the cooldown time
        {
            if (_difficulty == 1) //Human
            {
                _coolDown = 2;
            }
            else if (_difficulty == 2) //Bone Hunter
            {
                _coolDown = 1.5f;
            }
            else if (_difficulty == 3 || _difficulty == 4) //LBK and Must Die
            {
                _coolDown = 1;
            }

            attacking = false;
            _active = false;
        }

        public int GetDifficulty //Gets the difficulty from the game
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }

        private void AnimateFrame() //Handles the animations
        {
            if (_state == BossState.GroundIdle) //Idle
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
                    }
                    else
                    {
                        _frame++;
                    }
                    _frameTime = 0;
                }
            }
            else if (_state == BossState.Projectile) //Boss fire the projectile
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(50, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(178, 43, 119, 85);
                }
                else if (_frame == 2) //Projectile is fired on this frame
                {
                    _spriteFrame = new Rectangle(304, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(431, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(558, 43, 119, 85);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(687, 43, 119, 85);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(817, 43, 119, 85);
                }

                if (_frameTime >= 0.06)
                {
                    if (_frame >= 6)
                    {
                        _frame = 0;
                        StartCooldown();
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
                    }
                    else
                    {
                        _frame++;
                    }

                    _frameTime = 0;
                }
            }
            else if (_state == BossState.Hurt) //Taking damage
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

                if (_frameTime >= 0.1)
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
            else if (_state == BossState.Dead)
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(36, 43, 119, 85);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(164, 43, 106, 85);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(293, 43, 119, 85);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(422, 43, 119, 85);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(550, 43, 119, 85);
                }

                if (_frameTime >= 0.2)
                {
                    if (_frame < 4)
                    {
                        _frame++;
                        _frameTime = 0;
                    }
                    else if (_frame == 4)
                    {
                        bossDead = true;
                    }
                }
            }
        }

        private void GenerateBoxes() //Creates the hurtbox and hitboxes
        {
            if (_facingLeft)
            {
                //Hurtbox
                if (!damaged)
                {
                    bossHurtbox = new Rectangle(_location.X + 73, _location.Y + 22, _location.Width - 73, _location.Height - 22); //Hurtbox size never needs to change
                }
                //

                //Hitboxes
                if (_state == BossState.GroundIdle || _state == BossState.Jump)
                {
                    bossHitbox = Rectangle.Empty;
                }
                else if (_state == BossState.SlashOne || _state == BossState.SlashTwo)
                {
                    bossHitbox = new Rectangle(bossHurtbox.Left - 57, bossHurtbox.Y, 110, bossHurtbox.Height);
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
                //
            }
            else
            {
                //Hurtbox
                if (!damaged)
                {
                    bossHurtbox = new Rectangle(_location.X, _location.Y + 22, _location.Width - 73, _location.Height - 22); //Hurtbox size never needs to change
                }
                //

                //Hitboxes
                if (_state == BossState.GroundIdle || _state == BossState.Jump)
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
                //
            }
        }

        public void TakeDamage() //Boss takes damage
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
                _health -= 4;
            }

            if (_health <= 0)
            {
                _health = 0;
                _frame = 0;
                _state = BossState.Dead;
            }
            else if (_health > 0)
            {
                _state = BossState.Hurt;

                damaged = true;
                bossHurtbox = Rectangle.Empty;
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
