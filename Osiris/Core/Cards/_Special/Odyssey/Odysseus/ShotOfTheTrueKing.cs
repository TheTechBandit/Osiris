using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ShotOfTheTrueKing : BasicMove
    {
        public override string Name { get; } = "Shot of the True King";
        public override string Owner { get; } = "Odysseus";
        public override string Description { get; } = "Deal 12 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public ShotOfTheTrueKing() : base()
        {
            
        }

        public ShotOfTheTrueKing(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                var damage = 12;
                
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature}' aim holds true.");
            }
            
            inst.GetCardTurn().Actions--;
        }
        
    }
}