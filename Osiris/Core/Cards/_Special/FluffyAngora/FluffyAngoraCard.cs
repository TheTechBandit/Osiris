using System.Collections.Generic;

namespace Osiris
{
    public class FluffyAngoraCard : BasicCard
    {
        public override string Name { get; } = "Fluffy Angora";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();

        public FluffyAngoraCard() : base()
        {

        }

        public FluffyAngoraCard(bool newcard) : base(newcard)
        {
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/648638003752992771/angora.jpg";
            Moves.Add(new Swipe(true));
            Moves.Add(new SyrupSlide(true));
            Moves.Add(new Devour(true));
            Moves.Add(new StickyStomp(true));
            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}