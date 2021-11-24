using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTest1
{
    internal class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public Rectangle HitBox { get; set; }
        public AnimationFrame(Rectangle sourceRectangle)
        {
            SourceRectangle = sourceRectangle;
        }
    }
}