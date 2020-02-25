using System.Collections.Generic;

namespace Osiris
{
    public class CuteBunnyCard : BasicCard
    {
        public override string Name { get; } = "Cute Bunny";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; } = new List<BasicMove>();
        public override BasicPassive Passive { get; } = new SpareCarrotPassive(true);

        public CuteBunnyCard() : base()
        {

        }

        public CuteBunnyCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            
            Picture = "https://cdn.discordapp.com/attachments/460357767484407809/649081931383701504/cute_bun.png";
            Moves.Add(new Pomf(true));
            Moves.Add(new Kiss(true));
            Moves.Add(new Whiskers(true));
            Moves.Add(new Cottontail(true));
            TotalHP = 500;
            CurrentHP = 500;
        }
    }
}