using System.Collections.Generic;

namespace Osiris
{
    public class OHectorCard : BasicCard
    {
        public override string Name { get; set; } = "Hector, Champion of Troy";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public OHectorCard() : base()
        {

        }

        public OHectorCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682077950660575415/unknown.png";
            Moves.Add(new ChariotStrike(true));
            Moves.Add(new LanceRush(true));
            Moves.Add(new TrojanRoar(true));
            TotalHP = 500;
            CurrentHP = 500;

            DeathMessage = " has fallen to the Greek army!";
        }
    }
}