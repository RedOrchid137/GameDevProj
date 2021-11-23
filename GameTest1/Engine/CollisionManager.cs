using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace GameTest1.Engine
{
    public class CollisionManager
    {
        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.IntersectsWith(rect2))
            {
                Debug.WriteLine("collision");
                return true;
            }
            return false;
        }
    }
}
