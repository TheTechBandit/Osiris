using System.Collections.Generic;

namespace Osiris
{
    public class GhubCard : BasicCard
    {
        public override string Name { get; } = "Ghub";
        public override bool RequiresCelestial { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();

        public GhubCard() : base()
        {

        }

        public GhubCard(bool newcard) : base(newcard)
        {
            Moves.Add(new Chomp());
            Moves.Add(new Ghubs1911());
            Moves.Add(new EarFlap());
            Moves.Add(new DemigodOfEarth());
            Moves.Add(new GhubStomp());
            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}