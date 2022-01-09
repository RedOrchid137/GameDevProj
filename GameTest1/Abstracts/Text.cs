using GameTest1.Abstracts;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameTest1.Abstracts
{
    public abstract class Text : UIElement
    {
        private SpriteFont _font;

        public SpriteFont Font
        {
            get { return _font; }
        }

        protected string TextContent
        {
            get;set;
        }

        private Color _color;

        protected Color Color
        {
            get; set;
        }

        public Text(Rectangle window,SpriteBatch sb,SpriteFont font,Color color) : base(window, sb,font)
        {
            _font = font;
            _color = color;
        }

        public override void Draw(SpriteBatch spritebatch)
        { 
           spritebatch.DrawString(_font, TextContent, new Vector2(CurPosition.X, CurPosition.Y), _color);
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            return;
        }
    }
}
