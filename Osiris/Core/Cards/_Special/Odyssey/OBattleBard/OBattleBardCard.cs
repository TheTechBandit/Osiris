using System.Collections.Generic;

namespace Osiris
{
    public class OBattleBardCard : BasicCard
    {
        public override string Name { get; set; } = "Battle Bard";
        public override bool RequiresCelestial { get; } = false;
        public override bool Hidden { get; } = false;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new NonePassive(true);

        public OBattleBardCard() : base()
        {

        }

        public OBattleBardCard(bool newcard) : base(newcard)
        {
            HasUltimate = false;
            HasPassive = false;
            
            Picture = "https://cdn.discordapp.com/attachments/667958167078174771/683537159520780348/faw3cs1uhpw21.png";
            Moves.Add(new SharpNote(true));
            Moves.Add(new BattleTheme(true));
            Moves.Add(new IndimidationTactics(true));
            Moves.Add(new HappyDitty(true));
            TotalHP = 250;
            CurrentHP = 250;
        }
    }
}