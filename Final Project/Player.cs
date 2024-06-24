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
    public class Player
    {
        private List<Texture2D> _textures;
        private Rectangle _location;
        private Vector2 _speed;
        private float _spriteSeconds;
        private float _attackCoolDown;

        private Rectangle _spriteFrame;
        private float _frame;

        public Rectangle playerHitbox; //damage dealing area
        public Rectangle playerHurtbox; //damageable area

        PlayerState _state;

        private bool _facingLeft;
        private bool _facingRight = true;
        private bool _jumping;
        private bool _falling;
        public bool attacking = false;
        private bool _hitConnected = false;

        public bool playerDead;

        private int _difficulty; //1 = Human  2 = Bone Hunter  3 = LBK  4 = Must Die
        private int _health;

        public Player(List<Texture2D> textures, int x, int y)
        {
            _textures = textures;
            _location = new Rectangle(x, y, 100, 95);
            _speed = new Vector2();
            _state = PlayerState.Idle;
            _attackCoolDown = 1;
            _health = 100;
            playerDead = false;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, Boss boss, PlayerHealthBar playerHealthBar)
        {
            

            if (_state != PlayerState.Dead && !boss.bossDead) //If the fight is going on
            {
                if (_state != PlayerState.Hurt)
                {
                    if (!attacking) //Prevents idle and attack animations from overriding
                    {
                        _state = PlayerState.Idle;
                    }

                    //Jumping
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        if (!_jumping)
                        {
                            _state = PlayerState.Jumping;

                            _jumping = true;
                        }
                        else if (_jumping) //Jump is held
                        {
                            _state = PlayerState.Jumping;

                            if (_location.Y > 230 && !_falling) //Jumping
                            {
                                _speed.Y = -6;
                            }
                            else if (_location.Y <= 230 && !_falling || keyboardState.IsKeyUp(Keys.Space) && !_falling) //Jump peak reached
                            {
                                _falling = true;
                            }

                            if (_falling) //Started falling
                            {
                                _speed.Y = 6;

                                if (_location.Bottom >= 490) //Landed
                                {
                                    _speed.Y = 0;
                                    _falling = false;
                                    _jumping = false;
                                }
                            }
                        }
                    }

                    if (_jumping == true && keyboardState.IsKeyUp(Keys.Space)) //Jump is not held
                    {
                        _state = PlayerState.Jumping;

                        if (_location.Y > 320 && !_falling) //Jumping
                        {
                            _speed.Y = -6;
                        }
                        else if (_location.Y <= 320 && !_falling) //Reached jump peak
                        {
                            _falling = true;
                        }

                        if (_falling == true) //Falling back to the ground
                        {
                            _speed.Y = 6;

                            if (_location.Bottom >= 490) //Landed
                            {
                                _speed.Y = 0;
                                _location.Y = 490 - _location.Height;
                                _falling = false;
                                _jumping = false;
                            }
                        }
                    }
                    //

                    //Attacking
                    if (mouseState.LeftButton == ButtonState.Pressed && !attacking && _attackCoolDown == 0) //Attack started
                    {
                        _state = PlayerState.Attack;
                        _frame = 0;
                        attacking = true;
                        _attackCoolDown = 5;
                    }

                    if (attacking) //Attacking
                    {
                        _speed.X = 0;
                        _state = PlayerState.Attack;
                    }

                    if (playerHitbox.Intersects(boss.bossHurtbox)) //Hits the boss
                    {
                        boss.TakeDamage();
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
                    if (keyboardState.IsKeyDown(Keys.LeftShift) && keyboardState.IsKeyDown(Keys.A)) //Running left
                    {
                        _facingLeft = true;
                        _facingRight = false;

                        _speed.X = -8.5f;

                        if (_state != PlayerState.Attack && _state != PlayerState.Jumping) //Prevents animations from overriding
                        {
                            _state = PlayerState.Run;
                        }
                    }
                    else if (keyboardState.IsKeyDown(Keys.LeftShift) && keyboardState.IsKeyDown(Keys.D)) //Running right
                    {
                        _facingRight = true;
                        _facingLeft = false;

                        _speed.X = 8.5f;

                        if (_state != PlayerState.Attack && _state != PlayerState.Jumping) //Prevents animations from overriding
                        {
                            _state = PlayerState.Run;
                        }
                    }
                    else if (keyboardState.IsKeyDown(Keys.A)) //Walking left
                    {
                        _facingLeft = true;
                        _facingRight = false;

                        _speed.X = -4.5f;

                        if (_state != PlayerState.Attack && _state != PlayerState.Jumping) //Prevents animations from overriding
                        {
                            _state = PlayerState.Walk;
                        }
                    }
                    else if (keyboardState.IsKeyDown(Keys.D)) //Walking right
                    {
                        _facingRight = true;
                        _facingLeft = false;

                        _speed.X = 4.5f;

                        if (_state != PlayerState.Attack && _state != PlayerState.Jumping) //Prevents animations from overriding
                        {
                            _state = PlayerState.Walk;
                        }
                    }

                    if (_state == PlayerState.Idle) //No player input
                    {
                        _speed.X = 0;
                    }
                }
                else if (_state == PlayerState.Hurt)
                {
                    _speed.X = 0;
                }
            }
            else if (_state == PlayerState.Dead) //The player dies
            {
                _speed.X = 0;
                _speed.Y = 0;
                _location.Y = 490 - _location.Height;
                
                if (_facingLeft)
                {
                    _location.X = 400;
                }
                else if (_facingRight)
                {
                    _location.X = 400;
                }
            }
            else if (_state != PlayerState.Dead && boss.bossDead) //The boss is killed
            {
                _speed.X = 0;
                _speed.Y = 0;
                _location.Y = 490 - _location.Height;

                _state = PlayerState.Idle;
            }

            playerHealthBar.LoseHealth = ((float)_health / 100) * 155;
            playerHealthBar.Update();

            Move();
            GenerateBoxes();

            //Animation
            _spriteSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            AnimateFrame();
            //
        }

        private void AnimateFrame() //Handles the animations
        {
            if (_state == PlayerState.Walk) //Walk cycle
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(46, 53, 80, 75);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(178, 53, 80, 75);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(300, 53, 80, 75);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(425, 53, 80, 75);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(555, 53, 80, 75);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(689, 53, 80, 75);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(814, 53, 80, 75);
                }

                if (_spriteSeconds >= 0.08)
                {
                    if (_frame >= 6)
                    {
                        _frame = 0;
                    }
                    else
                    {
                        _frame++;
                    }

                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Idle) //Idle animation
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(38, 53, 80, 75);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(166, 53, 80, 75);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(294, 53, 80, 75);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(422, 53, 80, 75);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(550, 53, 80, 75);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(678, 53, 80, 75);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(806, 53, 80, 75);
                }

                if (_spriteSeconds >= 0.09)
                {
                    if (_frame >= 6)
                    {
                        _frame = 0;
                    }
                    else
                    {
                        _frame++;
                    }

                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Jumping) //Jump frame
            {
                _spriteFrame = new Rectangle(45, 53, 80, 75);
            }
            else if (_state == PlayerState.Run) //Run cycle
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(34, 53, 80, 75);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(163, 53, 80, 75);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(282, 53, 80, 75);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(414, 53, 80, 75);
                }
                else if (_frame == 4)
                {
                    _spriteFrame = new Rectangle(550, 53, 80, 75);
                }
                else if (_frame == 5)
                {
                    _spriteFrame = new Rectangle(676, 53, 80, 75);
                }
                else if (_frame == 6)
                {
                    _spriteFrame = new Rectangle(799, 53, 80, 75);
                }
                else if (_frame == 7)
                {
                    _spriteFrame = new Rectangle(931, 53, 80, 75);
                }

                if (_spriteSeconds >= 0.07)
                {
                    if (_frame >= 7)
                    {
                        _frame = 0;
                    }
                    else
                    {
                        _frame++;
                    }

                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Attack) //Attack animation
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(25, 53, 80, 75);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(159, 53, 80, 75);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(296, 53, 80, 75);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(424, 53, 80, 75);
                }

                if (_spriteSeconds >= 0.1)
                {
                    if (_frame == 3)
                    {
                        _frame = 0;
                        attacking = false;
                    }
                    else
                    {
                        _frame++;
                    }
                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Hurt) //Hurt animation
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(36, 53, 80, 75);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(164, 53, 80, 75);
                }

                if (_spriteSeconds >= 0.2)
                {
                    if (_frame == 1)
                    {
                        _frame = 0;
                        _state = PlayerState.Idle;
                    }
                    else
                    {
                        _frame++;
                    }

                    _spriteSeconds = 0;
                }
            }
            else if (_state == PlayerState.Dead) //Death animation
            {
                if (_frame == 0)
                {
                    _spriteFrame = new Rectangle(43, 53, 80, 75);
                }
                else if (_frame == 1)
                {
                    _spriteFrame = new Rectangle(172, 53, 80, 75);
                }
                else if (_frame == 2)
                {
                    _spriteFrame = new Rectangle(287, 53, 80, 75);
                }
                else if (_frame == 3)
                {
                    _spriteFrame = new Rectangle(404, 53, 80, 75);
                }
                
                if(_spriteSeconds >= 0.2)
                {
                    if (_frame == 3)
                    {
                        _frame = 3;
                        playerDead = true;
                    }
                    else
                    {
                        _frame++;
                    }
                    _spriteSeconds = 0;
                }
            }
        }

        private void Move() //Handles movement
        {
            _location.X += (int)_speed.X;
            _location.Y += (int)_speed.Y;

            if (_facingLeft)
            {
                if (_location.Left < -40) //Prevents the player from moving off the left egde of the screen
                {
                    _location.X = -40;
                }
                else if (_location.Right > 960) //Makes the player sprite stay against the right edge of the screen when turning there
                {
                    _location.X = 960 - _location.Width;
                }
            }
            else if (_facingRight)
            {
                if (_location.Left < 0) //Makes the player sprite stay against the left edge of the screen when turning there
                {
                    _location.X = 0;
                }
                else if (_location.Right > 1000) //Prevents the player from moving off the right edge of the screen
                {
                    _location.X = 1000 - _location.Width;
                }
            }
        }

        public int GetDifficulty //Gets the difficulty from the game
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }

        public void Damaged() //Player takes damage
        {
            if (_difficulty == 1) //Human
            {
                _health -= 4;
            }
            else if (_difficulty == 2) //Bone Hunter
            {
                _health -= 5;
            }
            else if (_difficulty == 3) //LBK
            {
                _health -= 10;
            }
            else if (_difficulty == 4) //Must Die
            {
                _health = 0;
            }

            if (_health <= 0) //Player is dead
            {
                _health = 0;
                _frame = 0;
                _state = PlayerState.Dead;
                playerHurtbox = Rectangle.Empty;
            }
            else if (_health > 0) //Player is hurt but not dead
            {
                _state = PlayerState.Hurt;
                _frame = 0;
                playerHurtbox = Rectangle.Empty;
            }
        }

        private void GenerateBoxes() //Generates the hurtbox and hitbox
        {
            //Hurtbox
            if (_state != PlayerState.Hurt && _state != PlayerState.Dead) //If the player is not currently being hurt or is not dead this creates the hurtbox
            {
                if (_facingLeft) //Player is facing left
                {
                    playerHurtbox = new Rectangle(_location.X + 40, _location.Y, _location.Width - 40, _location.Height);
                }
                else if (_facingRight) //Player is facing right
                {
                    playerHurtbox = new Rectangle(_location.X, _location.Y, _location.Width - 40, _location.Height);
                }
            }
            //

            //Hitbox
            if (_state == PlayerState.Attack && _facingRight) //Creates the hitbox to the right of the player
            {
                playerHitbox = new Rectangle(playerHurtbox.Right, _location.Y, 40, _location.Height);
            }
            else if (_state == PlayerState.Attack && _facingLeft) //Creates the hitbox to the left of the player
            {
                playerHitbox = new Rectangle(playerHurtbox.Left - 40, _location.Y, 40, _location.Height);
            }
            else //Removes the hitbox when the player isn't attacking
            {
                playerHitbox = Rectangle.Empty;
            }
            //
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_facingLeft) //Draws the player facing left
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White, 0f, new Vector2(0,0), SpriteEffects.FlipHorizontally, 0f);
            }
            else if (_facingRight) //Draws the player facing right
            {
                spriteBatch.Draw(_textures[(int)_state], _location, _spriteFrame, Color.White);
            }
        }
    }
}
