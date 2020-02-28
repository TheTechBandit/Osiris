using System.Collections.Generic;

namespace Osiris
{
    public class PriamCard : BasicCard
    {
        public override string Name { get; } = "Priam, King of Troy";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();
        public override BasicPassive Passive { get; } = new NonePassive(true);

        public PriamCard() : base()
        {

        }

        public PriamCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682274486363750481/th.png";
            Moves.Add(new Inspiring(true));
            Moves.Add(new Regent(true));
            Moves.Add(new BarterWithHades(true));
            Moves.Add(new HeelStrike(true));
            TotalHP = 600;
            CurrentHP = 600;
            DeathMessage = ", King of Troy, has fallen!";
        }
    }
}