using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.World
{
    public class Background:INeedsUpdate
    {
        private Texture2D _texture;
        public Background(Texture2D texture)
        {
            this._texture = texture;
            Scale = Level.screenHeight / this._texture.Height;
        }
        public float Scale { get; set; }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, new Rectangle(0, 0, (int)(_texture.Width*Scale), (int)(_texture.Height * Scale)), Color.White);
        }

        public void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
    }
}
