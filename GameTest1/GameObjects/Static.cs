using GameTest1.Animations;
using GameTest1.Extensions;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.GameObjects
{
    public class Static:Entity,INeedsUpdate
    {
        public Static(Texture2D texture,Rectangle window,float scale):base(texture,window, scale)
        {
        }
        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            this.CollisionRectangle = new Rectangle((int)CurPosition.X, (int)CurPosition.Y, (int)(Texture.Width*Scale), (int)(Scale*Texture.Height));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipFlagX)
            {
                spriteBatch.Draw(Texture, CurPosition, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            else
            {
                spriteBatch.Draw(Texture, CurPosition, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
        }


    }
}
