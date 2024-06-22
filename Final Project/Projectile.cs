using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Projectile
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;

        private bool _launched;
        private bool _firedLeft;
        private bool _firedRight;

        public Projectile(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 10, 10);
            _speed = new Vector2(0, 0);
            _firedLeft = false;
            _firedRight = false;
            _launched = false;
        }

        public void Update(GameTime gameTime, Player player, Boss boss)
        {
            if (!_launched)
            {
                _location = Rectangle.Empty;
            }
            else if (_firedLeft && _launched) //Projectile is fired left
            {
                _location = new Rectangle(boss.bossHurtbox.Left, boss.bossHurtbox.Y + 19, 10, 10);

                _firedLeft = false;
            }
            else if (_firedRight && _launched) //Projectile is fired right
            {
                _location = new Rectangle(boss.bossHurtbox.Right, boss.bossHurtbox.Y + 19, 10, 10);

                _firedRight = false;
            }
            else if (_launched && !_firedLeft && !_firedRight)
            {
                if (_location.Intersects(player.playerHurtbox))
                {
                    _location = Rectangle.Empty;
                    player.Damaged();
                }

                Move();
            }
        }

        public void FireLeft() //The boss fires from the right side of the screen
        {
            _firedLeft = true;
            _launched = true;

            _speed.X = -6;
        }

        public void FireRight()
        {
            _firedRight = true;
            _launched = true;

            _speed.X = 6;
        }

        private void Move()
        {
            _location.X += (int)_speed.X;

            if (_location.X < - 10 || _location.X > 960)
            {
                _speed.X = 0;
                _launched = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_launched)
            {
                spriteBatch.Draw(_texture, _location, Color.White);
            }
        }
    }
}
