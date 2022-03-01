using System;

namespace BirdBomber
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new BirdBomber())
                game.Run();
        }
    }
}
