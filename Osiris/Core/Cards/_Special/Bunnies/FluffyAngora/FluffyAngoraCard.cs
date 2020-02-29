using System.Collections.Generic;

namespace Osiris
{
    public class FluffyAngoraCard : BasicCard
    {
        public override string Name { get; set; } = "Fluffy Angora";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new FluffArmorPassive(true);

        public FluffyAngoraCard() : base()
        {

        }

        public FluffyAngoraCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;

            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/648638003752992771/angora.jpg";
            Moves.Add(new Poof(true));
            Moves.Add(new Rollout(true));
            Moves.Add(new Nuzzle(true));
            Moves.Add(new HairRaise(true));
            
            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}