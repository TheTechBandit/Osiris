using System;
using System.Collections.Generic;

namespace Osiris
{
    public class OSnakeCard : BasicCard
    {
        public override string Name { get; set; } = "Snake";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new CircesCursePassive(true);

        public OSnakeCard() : base()
        {

        }

        public OSnakeCard(bool newcard) : base(newcard)
        {
            IsPuppet = true;
            CanPassTurn = false;
            HasUltimate = false;
            
            Picture = "http://www.wallpaperup.com/uploads/wallpapers/2012/12/02/23594/fbce6f349a730a343fbd6c316c2c9da6.jpg";
            Moves.Add(new VenomSnap(true));
            Moves.Add(new Coil(true));

            DeathMessage = "'s transformation was reverted!";
            TotalHP = 75;
            CurrentHP = 75;
        }

    }
}