using GameTest1.Interfaces;
using Microsoft.Xna.Framework;

namespace GameTest1
{
    public interface IMobile
    {
        public Vector2 CurPosition { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Acceleration { get; set; }
        public IInputReader InputReader { get; set; }

        public void Move() { }
    }
}