using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    internal class Player
    {

        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;
        private float _seconds;

        private Rectangle _spriteFrame;


        public Player(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 53, 58);
            _speed = new Vector2();
        }

        public void Update(GameTime gameTime)
        {
            _seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            

        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            //spriteBatch.Draw(_textureList[PlayerState.Dead], _location, _spriteFrame, Color.White);
        }
    }
}
