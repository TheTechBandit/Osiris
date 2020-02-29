using System.Collections.Generic;

namespace Osiris
{
    public class PolyphemusBlindCard : BasicCard
    {
        public override string Name { get; set; } = "Polyphemus, Blinded";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public PolyphemusBlindCard() : base()
        {

        }

        public PolyphemusBlindCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/645751180688883712/682269858989211654/manny-rodriguez-official-cyclops-final.png";
            Moves.Add(new CrushBlind(true));
            Moves.Add(new SweepingArmBlind(true));
            Moves.Add(new MightOfTheGods(true));
            TotalHP = 400;
            CurrentHP = 400;
        }
    }
}