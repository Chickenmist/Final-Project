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

        public PlayerHealthBar(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 0, 0);
        }
    }
}
