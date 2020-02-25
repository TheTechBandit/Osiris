using System;
using System.Collections.Generic;

namespace Osiris
{
    public static class RandomGen
    {
        public static Random Gen { get; }

        static RandomGen()
        {
            Gen = new Random();
        }

        public static bool PercentChance(int chance)
        {
            int roll = Gen.Next(100);
            if(roll < chance)
                return true;
            else
                return false;
        }

        public static int RandomNum(int min, int max)
        {
            return Gen.Next(min, max+1);
        }

        //True = heads False = tails
        public static bool CoinFlip()
        {
            int flip = Gen.Next(1, 3);
            if(flip == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<int> RollDice(int d)
        {
            List<int> roll = new List<int>();
            roll.Add(Gen.Next(1, d+1));

            return roll;
        }

        public static List<int> RollDice(int d, bool crit)
        {
            int roll = Gen.Next(1, d+1);
            List<int> total = new List<int>();
            total.Add(roll);

            int count = 1;

            while(roll == d && count < 64)
            {
                roll = Gen.Next(1, d+1);
                total.Add(roll);
                count++;
            }

            return total;
        }

        public static List<int> RollDice(int num, int d)
        {
            List<int> rolls = new List<int>();

            for(int i = 0; i < num; i++)
            {
                rolls.Add(RollDice(d)[0]);
            }
            return rolls;
        }

        public static List<int> RollDice(int num, int d, bool crit)
        {
            List<int> rolls = new List<int>();

            for(int i = 0; i < num; i++)
            {
                foreach(int roll in RollDice(d, true))
                    rolls.Add(roll);
            }

            return rolls;
        }
    }
}