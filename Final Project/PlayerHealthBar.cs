using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class PlayerHealthBar
    {
        private Texture2D _texture;
        private Rectangle _location;
        private float _healthLeft;

        public PlayerHealthBar(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 155, 7);
            _healthLeft = 155;
        }

        public float LoseHealth
        {
            get {return _healthLeft; }
            set {_healthLeft = value; }
        }

        public void Update()
        {
            _location = new Rectangle(23, 25, (int)_healthLeft, 7);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }
    }
}
