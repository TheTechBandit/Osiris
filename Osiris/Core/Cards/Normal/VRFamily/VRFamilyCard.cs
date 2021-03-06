using System.Collections.Generic;

namespace Osiris
{
    public class VRFamilyCard : BasicCard
    {
        public override string Name { get; set; } = "VRFamily";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public VRFamilyCard() : base()
        {

        }

        public VRFamilyCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Moves.Add(new Crash(true));
            Moves.Add(new Jaunt(true));
            Moves.Add(new StalwartSoul(true));
            Moves.Add(new DeRez(true));

            TotalHP = 250;
            CurrentHP = 250;
        }
    }
}