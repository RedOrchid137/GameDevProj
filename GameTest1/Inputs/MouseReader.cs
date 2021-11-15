using GameTest1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Inputs
{
    public class MouseReader:IInputReader
    {
        public bool IsDestinationInput => false;

        public Vector2 ReadInput()
        {
            MouseState state = Mouse.GetState();
            Vector2 positieMuis = new Vector2(state.X, state.Y);
            return positieMuis;
        }
    }

}
