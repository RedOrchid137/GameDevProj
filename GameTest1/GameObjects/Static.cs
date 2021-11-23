using GameTest1.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.GameObjects
{
    public class Static:GameObject,INeedsUpdate
    {
        public Static(Spritesheet ss,SpriteBatch sb,Rectangle window,float scale):base(ss,sb, window, scale)
        {
            this.CollisionRectangle = new Rectangle((int)Position.X,(int)Position.Y,Texture.Width,Texture.Height);
        }

        public void Update(GameTime gametime)
        {
        }
        public void Draw()
        {
            _spriteBatch.Begin();
            if (FlipFlagX)
            {
                _spriteBatch.Draw(_texture, Position,null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                _spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
            _spriteBatch.End();
        }


    }
}
