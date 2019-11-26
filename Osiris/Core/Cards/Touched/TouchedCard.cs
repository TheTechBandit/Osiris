using System.Collections.Generic;

namespace Osiris
{
    public class TouchedCard : BasicCard
    {
        public override string Name { get; } = "Touched";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();

        public TouchedCard() : base()
        {

        }

        public TouchedCard(bool newcard) : base(newcard)
        {
            Moves.Add(new Strike(true));
            Moves.Add(new RestingSanctuary(true));
            Moves.Add(new DesperateFlurry(true));
            Moves.Add(new Rush(true));
            TotalHP = 250;
            CurrentHP = 250;
        }
    }
}