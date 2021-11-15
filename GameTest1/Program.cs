using System;

namespace GameTest1
{
    public static class Program
    {
        public static Random rangen = new Random();
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
