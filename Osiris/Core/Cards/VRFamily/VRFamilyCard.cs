using System.Collections.Generic;

namespace Osiris
{
    public class VRFamilyCard : BasicCard
    {
        public override string Name { get; } = "VRFamily";
        public override bool RequiresCelestial { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();

        public VRFamilyCard() : base()
        {

        }

        public VRFamilyCard(bool newcard) : base(newcard)
        {
            Moves.Add(new Crash());
        }
    }
}