using System.Collections.Generic;

namespace Osiris
{
    public class OdysseusOldCard : BasicCard
    {
        public override string Name { get; } = "Odysseus, Old";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();
        public override BasicPassive Passive { get; } = new NonePassive(true);

        public OdysseusOldCard() : base()
        {

        }

        public OdysseusOldCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682271164588228635/2bcfd5db6dd1f86fa8f3915215cb708d.png";
            Moves.Add(new ShotOfTheTrueKing(true));

            TotalHP = 10;
            CurrentHP = 10;
        }
    }
}