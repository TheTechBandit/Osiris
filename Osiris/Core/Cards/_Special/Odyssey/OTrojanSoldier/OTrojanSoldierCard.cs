using System.Collections.Generic;

namespace Osiris
{
    public class OTrojanSoldierCard : BasicCard
    {
        public override string Name { get; set; } = "Trojan Soldier";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public OTrojanSoldierCard() : base()
        {

        }

        public OTrojanSoldierCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/682064436927660295/1703925ec2e7220367de1bbb0d5d8588.png";
            Moves.Add(new Stab(true));
            Moves.Add(new ShieldBlock(true));
            TotalHP = 100;
            CurrentHP = 100;
        }
    }
}