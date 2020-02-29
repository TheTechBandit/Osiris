using System.Collections.Generic;

namespace Osiris
{
    public class AngryJackalopeCard : BasicCard
    {
        public override string Name { get; set; } = "Angry Jackalope";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new HerdLeaderPassive(true);

        public AngryJackalopeCard() : base()
        {

        }

        public AngryJackalopeCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/647278129798184966/jackalope_by_mickehill-da3y98v.png";
            Moves.Add(new Gore(true));
            Moves.Add(new Impale(true));
            Moves.Add(new RackTap(true));
            Moves.Add(new Plow(true));
            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}