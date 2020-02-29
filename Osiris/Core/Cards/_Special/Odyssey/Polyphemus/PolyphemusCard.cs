using System.Collections.Generic;

namespace Osiris
{
    public class PolyphemusCard : BasicCard
    {
        public override string Name { get; set; } = "Polyphemus, Son of Poseidon";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public PolyphemusCard() : base()
        {

        }

        public PolyphemusCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682269438346788866/cyclops01.png";
            Moves.Add(new Crush(true));
            Moves.Add(new SweepingArm(true));
            Moves.Add(new MightOfTheGods(true));
            TotalHP = 150;
            CurrentHP = 150;

            DeathMessage =  " has been Blinded by the party!";
        }
    }
}