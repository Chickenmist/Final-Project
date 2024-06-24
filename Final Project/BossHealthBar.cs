using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class BossHealthBar
    {
        private Texture2D _texture;
        private Rectangle _location;
        private float _healthLeft;

        public BossHealthBar(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 690, 10);
            _healthLeft = 690;
        }

        public float LoseHealth
        {
            get { return _healthLeft; }
            set { _healthLeft = value; }
        }

        public void Update()
        {
            _location = new Rectangle(136, 520, (int)_healthLeft, 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
