using System;

namespace DiscomonProject
{
    public static class RandomGen
    {
        public static Random Gen { get; }

        static RandomGen()
        {
            Gen = new Random();
        }

        public static int RollDice(int d)
        {
            return Gen.Next(1, d+1);
        }

        public static int RollDice(int num, int d)
        {
            int result = 0;
            for(int i = 0; i < num; i++)
            {
                result += RollDice(d);
            }
            return result;
        }
    }
}