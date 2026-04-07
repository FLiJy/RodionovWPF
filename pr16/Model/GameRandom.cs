using System;
using pr16;

namespace pr16
{
    public static class GameRandom
    {
        public static Random rnd = new Random();

        public static bool NextBool()
        {
            return rnd.Next(2) == 0;
        }
    }
}