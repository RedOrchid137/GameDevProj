using System;
using System.Collections.Generic;
using System.Text;
using GameTest1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static System.Windows.Forms.AxHost;

namespace GameTest1.Inputs
{
    internal class KeyboardReader:IInputReader
    { 

        public bool IsDestinationInput => true;
        public KeyboardState prevState;
        public KeyboardState state;
        public bool InputDifferent { get; set; }
        public Vector2 ReadInput()
        {
            state = Keyboard.GetState();
            if (prevState != state)
            {
                InputDifferent = true;
            }
            else
            {
                InputDifferent = false;
            }
            Vector2 direction = Vector2.Zero;
            if (state.IsKeyDown(Keys.Left))
            {
                direction.X -= 1;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                direction.X += 1;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                direction.Y += 1;
            }
            prevState = state;
            return direction;

        }
    }
}
