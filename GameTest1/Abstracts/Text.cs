using GameTest1.Abstracts;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

        private string _textcontent;

        protected string TextContent
        {
            get;set;
        }

        private Color _color;

        protected Color Color
        {
            get; set;
        }

        private Rectangle _rect;
        protected Rectangle Rectangle
        {
            get;set;
        }

        public Text(Rectangle window,SpriteBatch sb,SpriteFont font,Color color,Vector2 dimensions) : base(window, sb,font)
        {
            _font = font;
            _color = color;
            _rect.Width = (int)dimensions.X;
            _rect.Height = (int)dimensions.Y;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            _rect = new Rectangle((int)CurPosition.X, (int)CurPosition.Y,Rectangle.Width,Rectangle.Height);
            if (!string.IsNullOrEmpty(_textcontent))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(_textcontent).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(_textcontent).Y / 2);

                spritebatch.DrawString(_font, _textcontent, new Vector2(x, y), _color);
            }
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            return;
        }
    }
}
