using System.Collections.Generic;

namespace Osiris
{
    public class GhubCard : BasicCard
    {
        public override string Name { get; } = "Ghub";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();

        public GhubCard() : base()
        {

        }

        public GhubCard(bool newcard) : base(newcard)
        {
            HasPassive = false;
            
            Moves.Add(new Chomp(true));
            Moves.Add(new Ghubs1911(true));
            Moves.Add(new EarFlap(true));
            Moves.Add(new DemigodOfEarth(true));
            Moves.Add(new GhubStomp(true));
            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}