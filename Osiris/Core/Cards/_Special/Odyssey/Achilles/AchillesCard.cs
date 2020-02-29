using System.Collections.Generic;

namespace Osiris
{
    public class AchillesCard : BasicCard
    {
        public override string Name { get; set; } = "Achilles";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get;set;  } = new BlessingOfStyxPassive(true);

        public AchillesCard() : base()
        {

        }

        public AchillesCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682273499619786787/Hector_vs_Achilles_by_GENZOMAN.png";
            Moves.Add(new HavocSpear(true));
            Moves.Add(new SweepingBlow(true));
            Moves.Add(new ChampionOfTheGods(true));
            TotalHP = 1000;
            CurrentHP = 1000;

            DeathMessage = "' secret weakness was found! Achilles has been slain!";
        }
    }
}