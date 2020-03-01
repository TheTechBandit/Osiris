using System.Collections.Generic;

namespace Osiris
{
    public class CharybdisCard : BasicCard
    {
        public override string Name { get; set; } = "Charybdis, Maw of the Deep";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public CharybdisCard() : base()
        {

        }

        public CharybdisCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/683418839257841720/fefba813de8ee2899572d0e4ca39f8b3.png";
            Moves.Add(new WaterVortex(true));
            Moves.Add(new CrushingTeeth(true));
            Moves.Add(new TorrentFury(true));
            Moves.Add(new Swallow(true));
            TotalHP = 2000;
            CurrentHP = 2000;

            DeathMessage = " retreats into the deep...";
        }
    }
}