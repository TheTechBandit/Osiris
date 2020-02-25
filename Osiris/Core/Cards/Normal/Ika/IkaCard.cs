using System.Collections.Generic;

namespace Osiris
{
    public class IkaCard : BasicCard
    {
        public override string Name { get; } = "Ika";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();
        public override BasicPassive Passive { get; } = new HerdLeaderPassive(true);

        public IkaCard() : base()
        {

        }

        public IkaCard(bool newcard) : base(newcard)
        {
            HasPassive = false;
            HasUltimate = false;
            
            Moves.Add(new HystericalLaughter(true));

            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}