using System.Collections.Generic;

namespace Osiris
{
    public class OArcherCard : BasicCard
    {
        public override string Name { get; } = "Archer";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();
        public override BasicPassive Passive { get; } = new NonePassive(true);

        public OArcherCard() : base()
        {

        }

        public OArcherCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://media.discordapp.net/attachments/460357767484407809/681702626718646293/b026f166221cde3888cd712fd95c8404.png";
            Moves.Add(new QuickShot(true));
            Moves.Add(new ArrowOfHermes(true));
            Moves.Add(new HealingFlask(true));
            Moves.Add(new FiringPosition(true));
            TotalHP = 250;
            CurrentHP = 250;
        }
    }
}