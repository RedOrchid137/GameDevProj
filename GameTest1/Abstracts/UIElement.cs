using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Abstracts
{
    public abstract class UIElement:INeedsUpdate
    {
        protected SpriteBatch _spriteBatch;
        protected Texture2D _texture;
        protected Rectangle _window;

        public Vector2 CurPosition { get; set; }
        public bool FlipFlagX { get; set; } = false;
        public bool FlipFlagY { get; set; } = false;
        public float Scale { get; set; }
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }
        public Rectangle Window { get { return _window; } set { _window = value; } }
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }

        public abstract void Draw(SpriteBatch spritebatch);
        public abstract void Update(GameTime gametime, Level curLevel, SpriteBatch sb);

        public UIElement(Texture2D texture,Rectangle window, SpriteBatch sb,float scale)
        {
            this.Texture = texture;
            this.Window = window;
            this.SpriteBatch = sb;
            this.Scale = scale;
        }
        public UIElement(Rectangle window, SpriteBatch sb)
        {
            this.Window = window;
            this.SpriteBatch = sb;
        }

    }
}
