using System.Collections.Generic;

namespace Osiris
{
    public class OKegmasterCard : BasicCard
    {
        public override string Name { get; set; } = "Kegmaster";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public OKegmasterCard() : base()
        {

        }

        public OKegmasterCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/681705068906414247/ancient_greeklegends29.png";
            Moves.Add(new Smash(true));
            Moves.Add(new GreekFire(true));
            Moves.Add(new GunpowderKeg(true));
            Moves.Add(new WineBarrel(true));
            TotalHP = 250;
            CurrentHP = 250;
        }
    }
}