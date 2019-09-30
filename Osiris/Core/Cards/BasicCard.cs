using System.Collections.Generic;

namespace Osiris
{
    public class BasicCard
    {
        public virtual string Name { get; }
        public virtual bool RequiresCelestial { get; }
        public virtual List<BasicMove> Moves { get; }
        public bool Dead { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }

        public BasicCard()
        {

        }

        public BasicCard(bool newcard)
        {
            Dead = false;
            TotalHP = 500;
            CurrentHP = 500;
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
    }
}