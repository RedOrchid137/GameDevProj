using GameTest1.Abstracts;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.UI
{
    public class Text : UIElement
    {
        private SpriteFont _font;

        public SpriteFont Font
        {
            get { return _font; }
        }



        public Text(Rectangle window, SpriteBatch sb,SpriteFont font) : base(window, sb)
        {

        }

        public override void Draw(SpriteBatch spritebatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
    }
}
