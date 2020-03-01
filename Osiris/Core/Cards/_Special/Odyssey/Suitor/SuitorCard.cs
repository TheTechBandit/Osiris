using System.Collections.Generic;

namespace Osiris
{
    public class SuitorCard : BasicCard
    {
        public override string Name { get; set; } = "Suitor";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public SuitorCard() : base()
        {

        }

        public SuitorCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/667958167078174771/683729206357655637/db2eaa4a3682bf58be49d1d762371ebf.png";
            Moves.Add(new Throw(true));
            Moves.Add(new Punch(true));
            Moves.Add(new Flee(true));
            TotalHP = 115;
            CurrentHP = 115;
        }
    }
}