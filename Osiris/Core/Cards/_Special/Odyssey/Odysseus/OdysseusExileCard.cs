using System.Collections.Generic;

namespace Osiris
{
    public class OdysseusExileCard : BasicCard
    {
        public override string Name { get; } = "Odysseus, Exiled";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();
        public override BasicPassive Passive { get; } = new OutcastOfTheGodsPassive(true);

        public OdysseusExileCard() : base()
        {

        }

        public OdysseusExileCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682269059764584455/1c1cb195afa556aaf9fd4ca145ed87b9--greek-history-greek-mythology.png";
            Moves.Add(new BladeOfAresExiled(true));
            Moves.Add(new IthacanWrath(true));
            Moves.Add(new ArtemisBarrageExiled(true));
            Moves.Add(new OdysseusImpaleExiled(true));

            TotalHP = 1000;
            CurrentHP = 1000;
        }
    }
}