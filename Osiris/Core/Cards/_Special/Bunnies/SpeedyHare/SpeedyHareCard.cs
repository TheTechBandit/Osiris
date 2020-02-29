using System.Collections.Generic;

namespace Osiris
{
    public class SpeedyHareCard : BasicCard
    {
        public override string Name { get; set; } = "Speedy Hare";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new SpeederPassive(true);

        public SpeedyHareCard() : base()
        {

        }

        public SpeedyHareCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/648753951583371308/speedster.jpg";
            Moves.Add(new Prance(true));
            Moves.Add(new Reel(true));
            Moves.Add(new ThumperKicks(true));
            Moves.Add(new XLR8(true));

            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}