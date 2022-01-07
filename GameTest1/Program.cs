using System;

namespace GameTest1
{
    public static class Program
    {
        public static Random rangen = new Random();
        public static GameBase CurGame;
        [STAThread]
        static void Main()
        {
            using (var game = new Game2())
            {
                CurGame = game;
                game.Run();
            }

        }
    }
}
