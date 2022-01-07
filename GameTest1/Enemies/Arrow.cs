using GameTest1.Extensions;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Enemies
{
    public class Arrow : Entity
    {

        public HunterEnemy Hunter { get; set; }
        public Arrow(Texture2D texture, Rectangle window, float scale) : base(texture, window, scale)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)CurPosition.X, (int)CurPosition.Y, Texture.Width, Texture.Height), Color.White);
            spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {

        }
    }
}
