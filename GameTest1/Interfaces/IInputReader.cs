using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Interfaces
{
    public interface IInputReader
    {
        public Vector2 ReadInput();
        public bool IsDestinationInput { get;}
    }
}
