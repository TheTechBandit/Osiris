using System.Collections.Generic;

namespace Osiris
{
    public class TouchedCard : BasicCard
    {
        public override string Name { get; } = "Touched";
        public override bool RequiresCelestial { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();

        public TouchedCard() : base()
        {

        }

        public TouchedCard(bool newcard) : base(newcard)
        {
            Moves.Add(new Strike());
            Moves.Add(new RestingSanctuary());
            Moves.Add(new DesperateFlurry());
            Moves.Add(new Rush());
            TotalHP = 250;
            CurrentHP = 250;
        }
    }
}