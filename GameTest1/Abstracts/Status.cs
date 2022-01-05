using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.State
{
    public abstract class Status
    {
        protected ContentManager _content;

        protected GraphicsDevice _graphicsDevice;

        protected GameBase _spel;
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        

        public Status(GameBase spel, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _spel = spel;

            _graphicsDevice = graphicsDevice;

            _content = content;
        }

        public abstract void Update(GameTime gameTime);

        
    }
}

