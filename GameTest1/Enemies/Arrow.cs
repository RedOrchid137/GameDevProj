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

        public HunterEnemy hunter { get; set; }
        public Arrow(Texture2D texture, Rectangle window, float scale) : base(texture, window, scale)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {

        }
    }
}
