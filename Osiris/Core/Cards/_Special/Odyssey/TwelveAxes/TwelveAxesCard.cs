using System.Collections.Generic;

namespace Osiris
{
    public class TwelveAxesCard : BasicCard
    {
        public override string Name { get; set; } = "Twelve Axe Heads";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public TwelveAxesCard() : base()
        {

        }

        public TwelveAxesCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/683751643791097975/OdysseusBow.png";
            TotalHP = 12;
            CurrentHP = 12;

            DeathMessage = " challenge was completed by Odysseus!";
        }

        public override List<int> TakeDamage(int damage)
        {
            if(damage == 12)
            {
                CurrentHP = 0;
            }

            return new List<int>();
        }
    }
}