using System.Collections.Generic;

namespace Osiris
{
    public class OWarriorCard : BasicCard
    {
        public override string Name { get; set; } = "Warrior";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public OWarriorCard() : base()
        {

        }

        public OWarriorCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/681703391772016659/4a95591f52f7d624de62dd9d5a5b0cdf.png";
            Moves.Add(new Slash(true));
            Moves.Add(new StunningPunch(true));
            Moves.Add(new AresFury(true));
            Moves.Add(new Blockade(true));
            TotalHP = 300;
            CurrentHP = 300;
        }
    }
}