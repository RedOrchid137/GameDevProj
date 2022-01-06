using GameTest1.Animations;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Entities
{
    public class Collectible : Entity
    {
        public Collectible(Spritesheet spritesheet, Rectangle window, Level curlevel, Vector2 startingtile, float scale = 1, float maxSpeed = 0) : base(spritesheet, window, curlevel, startingtile, scale, maxSpeed)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
    }
}
