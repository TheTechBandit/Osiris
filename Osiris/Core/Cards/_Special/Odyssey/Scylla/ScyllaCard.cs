using System.Collections.Generic;

namespace Osiris
{
    public class ScyllaCard : BasicCard
    {
        public override string Name { get; set; } = "Scylla Head, Terror of the Cave";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public ScyllaCard() : base()
        {

        }

        public ScyllaCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/683418928932061246/0039cc9122558ce45f9e0eeb2493f73a.png";
            Moves.Add(new GruelingSnap(true));
            Moves.Add(new Bloodsong(true));
            Moves.Add(new BatteringBlow(true));
            TotalHP = 400;
            CurrentHP = 400;

            DeathMessage = " has been slain! Scylla screeches in anger.";
        }
    }
}