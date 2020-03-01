using System;
using System.Collections.Generic;

namespace Osiris
{
    public class OKingfisherCard : BasicCard
    {
        public override string Name { get; set; } = "Kingfisher";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new CircesCursePassive(true);

        public OKingfisherCard() : base()
        {

        }

        public OKingfisherCard(bool newcard) : base(newcard)
        {
            IsPuppet = true;
            CanPassTurn = false;
            HasUltimate = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/683711488463994901/thumb-350-841692.png";
            Moves.Add(new EggDrop(true));
            Moves.Add(new Tweet(true));

            DeathMessage = "'s transformation was reverted!";
            TotalHP = 50;
            CurrentHP = 50;
        }

    }
}