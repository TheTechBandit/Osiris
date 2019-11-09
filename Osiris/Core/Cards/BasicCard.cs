using System.Collections.Generic;
using System.Drawing;

namespace Osiris
{
    public class BasicCard
    {
        public virtual string Name { get; }
        public virtual bool RequiresCelestial { get; }
        public virtual List<BasicMove> Moves { get; }
        public ulong Owner { get; set; }
        public string Signature { get; set; }
        public bool Dead { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }
        public bool IsTurn { get; set; }

        public BasicCard()
        {

        }

        public BasicCard(bool newcard)
        {
            Owner = 0;
            Signature = Name;
            Dead = false;
            TotalHP = 500;
            CurrentHP = 500;
            IsTurn = false;
        }

        public void CooldownTickdown()
        {
            foreach(BasicMove move in Moves)
            {
                move.CooldownTick();
            }
        }

        public void CooldownsReset()
        {
            foreach(BasicMove move in Moves)
            {
                move.CooldownReset();
            }
        }

        public List<int> HPGradient()
        {
            double perc = (double)CurrentHP/(double)TotalHP;
            perc *= 100;
            int g = 2*(int)perc;
            int r = 200-g;
            int b = 0;

            return new List<int>()
            {
                r, g, b,
            };
        }

        public BasicMove ParseMove(string str)
        {
            str = str.ToLower();
            foreach(BasicMove move in Moves)
            {
                if(move.Name.ToLower() == str)
                {
                    return move;
                }
            }
            return null;
        }
    }
}