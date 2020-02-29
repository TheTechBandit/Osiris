using System.Collections.Generic;

namespace Osiris
{
    public class SugarGhubbyCard : BasicCard
    {
        public override string Name { get; set; } = "Sugar Ghubby";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new GhubbleTroublePassive(true);

        public SugarGhubbyCard() : base()
        {

        }

        public SugarGhubbyCard(bool newcard) : base(newcard)
        {            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/648290945162543124/Sugar_Ghubby.jpg";
            Moves.Add(new Swipe(true));
            Moves.Add(new SyrupSlide(true));
            Moves.Add(new Devour(true));
            Moves.Add(new StickyStomp(true));
            TotalHP = 1000;
            CurrentHP = 1000;
        }
    }
}