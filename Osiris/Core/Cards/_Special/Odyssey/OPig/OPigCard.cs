using System;
using System.Collections.Generic;

namespace Osiris
{
    public class OPigCard : BasicCard
    {
        public override string Name { get; set; } = "Pig";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new CircesCursePassive(true);

        public OPigCard() : base()
        {

        }

        public OPigCard(bool newcard) : base(newcard)
        {
            IsPuppet = true;
            CanPassTurn = false;
            HasUltimate = false;
            
            Picture = "https://i.pinimg.com/originals/07/f2/7f/07f27f8d96e229375dabb183268b9217.jpg";
            Moves.Add(new TuskRush(true));
            Moves.Add(new Squeal(true));

            DeathMessage = "'s transformation was reverted!";
            TotalHP = 120;
            CurrentHP = 10;
        }

    }
}