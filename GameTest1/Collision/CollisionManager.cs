using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameTest1.Collision
{
    class CollisionManager
    {
        public bool CheckCollsion(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.IntersectsWith(rect2))
            {
                return true;
            }
            return false;
        }
    }
}
