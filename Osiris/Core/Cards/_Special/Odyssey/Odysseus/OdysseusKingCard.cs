using System.Collections.Generic;

namespace Osiris
{
    public class OdysseusKingCard : BasicCard
    {
        public override string Name { get; } = "Odysseus, King of Ithaca";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();
        public override BasicPassive Passive { get; } = new WarriorOfAthenaPassive(true);

        public OdysseusKingCard() : base()
        {

        }

        public OdysseusKingCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682270164397457408/1747157-odysseus2.png";
            Moves.Add(new BladeOfAres(true));
            Moves.Add(new IthacanWrath(true));
            Moves.Add(new ArtemisBarrage(true));
            Moves.Add(new OdysseusImpale(true));

            TotalHP = 1000;
            CurrentHP = 1000;
        }
    }
}