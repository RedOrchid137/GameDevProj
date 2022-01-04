using GameTest1.Engine;
using GameTest1.Sprites;
using GameTest1.World;
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
              -Level.screenHeight / 2,
              0);

            var offset = Matrix.CreateTranslation(
                Level.screenWidth/2,
                Level.screenHeight/2,
                0);

            Transform = position * offset;
        }
    }
}
