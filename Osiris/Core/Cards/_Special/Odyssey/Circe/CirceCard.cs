using System.Collections.Generic;

namespace Osiris
{
    public class CirceCard : BasicCard
    {
        public override string Name { get; set; } = "Circe, Temptress of the Mountain";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public CirceCard() : base()
        {

        }

        public CirceCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "http://4.bp.blogspot.com/-KIVa86tGFrM/VfbYkRkI8nI/AAAAAAAABwI/3ptgQWWaYUQ/s1600/circe_2_by_kevinnichols-d60fbkc.jpg";
            Moves.Add(new StaffBlast(true));
            Moves.Add(new Mending(true));
            Moves.Add(new HoneyedWine(true));

            TotalHP = 550;
            CurrentHP = 550;
        }
    }
}