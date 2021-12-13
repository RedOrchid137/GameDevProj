using GameTest1.Engine;
using GameTest1.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Perspective
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Character target)
        {
            var position = Matrix.CreateTranslation(
              -target.CurPosition.X - (target.curAnimation.CurrentFrame.SourceRectangle.Width / 2),
              -target.CurPosition.Y - (target.curAnimation.CurrentFrame.SourceRectangle.Height / 2),
              0);

            var offset = Matrix.CreateTranslation(
                World.screenWidth / 2,
                World.screenHeight / 2,
                0);

            Transform = position * offset;
        }
    }
}
