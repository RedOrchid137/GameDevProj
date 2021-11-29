using GameTest1.Animations;
using GameTest1.Extensions;
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
            this.CollisionRectangle = new Rectangle((int)CurPosition.X, (int)CurPosition.Y, (int)(Texture.Width*Scale), (int)(Scale*Texture.Height));
        }
        public override void Draw()
        {
            _spriteBatch.Begin();
            if (FlipFlagX)
            {
                _spriteBatch.Draw(_texture, CurPosition, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            else
            {
                _spriteBatch.Draw(_texture, CurPosition, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            _spriteBatch.End();
        }


    }
}
