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
        }

        public override void Update(GameTime gametime)
        {
            this.CollisionRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width*Scale), (int)(Scale*Texture.Height));
        }
        public override void Draw()
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
