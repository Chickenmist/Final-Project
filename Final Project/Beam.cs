using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Beam
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Rectangle _horizontalHitBox;
        private bool _launchedHorizontal;
        private bool _leftSide;
        private bool _launchedVertical;
        private bool _verticalLaunchStart;

        private Rectangle _spriteFrame;

        private List<Rectangle> beams = new List<Rectangle>(); //This is for the vertical beams

        private float _timeActive;

        public Beam(Texture2D texture, int x, int y)
        {
            _texture = texture;
            _location = new Rectangle(x, y, 887, 100);
        }

        public void Update(GameTime gameTime, Boss boss, Player player)
        {
            if (_launchedHorizontal)
            {
                if (_leftSide) //Boss is on the left side of the screen
                {
                    _location = new Rectangle( boss.bossHurtbox.Right, boss.bossHurtbox.Y, 887, 100);
                }
                else //Boss is on the right side of the screen
                {
                    _location = new Rectangle(boss.bossHurtbox.Left - 887, boss.bossHurtbox.Y, 887, 100);
                }
                _spriteFrame = new Rectangle(144, 26, 73, 21);

                _timeActive = 0;
            }
            else if (_verticalLaunchStart)
            {
                _spriteFrame = new Rectangle(72, 21, 36, 29);
            }
            else if (_launchedVertical)
            {
                _spriteFrame = new Rectangle(144, 26, 73, 21);

                _timeActive = 0;
            }

            if (_horizontalHitBox.Intersects(player.playerHurtbox)) //Damages the player during the horizontal beam
            {
                player.Damaged();
            }

            foreach (var beam in beams) //Damage the player during the vertical beam
            {
                if(beam.Intersects(player.playerHurtbox))
                {
                    player.Damaged();
                }
            }

            if (_timeActive < 2)
            {
                _timeActive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (_timeActive >= 2)
            {
                _timeActive = 2;
            }

            GenerateHitboxes();
        }

        public void FiredHorizontalLeft() //Launches the beam horizontally
        {
            _launchedHorizontal = true;
            _leftSide = true;
        }

        public void FiredHorizontalRight()
        {
            _launchedHorizontal = true;
            _leftSide = false;
        }

        public void FiredVertical() //Launches the beam vertically
        {
            _launchedVertical = true;
            beams.Clear();
            _verticalLaunchStart = false;
        }

        public void PrepVertical()
        {
            _verticalLaunchStart = true;
        }

        private void GenerateHitboxes()
        {
            if (_timeActive != 2)
            {
                if (_launchedHorizontal)
                {
                    _horizontalHitBox = new Rectangle(_location.X, _location.Y, _location.Width, _location.Height);
                }
                else if (_verticalLaunchStart)
                {
                    beams.Add(new Rectangle(0, 500, 100, 30));
                    beams.Add(new Rectangle(200, 500, 100, 30));
                    beams.Add(new Rectangle(400, 500, 100, 30));
                    beams.Add(new Rectangle(600, 500, 100, 30));
                    beams.Add(new Rectangle(800, 500, 100, 30));
                }
                else if (_launchedVertical)
                {
                    beams.Add(new Rectangle(0, 0, 100, 560));
                    beams.Add(new Rectangle(200, 0, 100, 560));
                    beams.Add(new Rectangle(400, 0, 100, 560));
                    beams.Add(new Rectangle(600, 0, 100, 560));
                    beams.Add(new Rectangle(800, 0, 100, 560));
                }
            }
            else
            {
                _horizontalHitBox = Rectangle.Empty;
                beams.Clear();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_launchedHorizontal)
            {
                spriteBatch.Draw(_texture, _location, _spriteFrame, Color.White);
            }
            else if (_launchedVertical)
            {
                foreach (var beams in beams)
                {
                    spriteBatch.Draw(_texture, beams, _spriteFrame, Color.White, 270, new Vector2(_spriteFrame.Width / 2f, _spriteFrame.Height / 2f), SpriteEffects.None, 0f);
                }
            }
        }
    }
}
