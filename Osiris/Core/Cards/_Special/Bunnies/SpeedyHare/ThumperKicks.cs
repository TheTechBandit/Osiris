using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ThumperKicks : BasicMove
    {
        public override string Name { get; } = "Thumper Kicks";
        public override string Owner { get; } = "Speedy Hare";
        public override string Description { get; } = "Rapid-Fire kicks! Deal 12d2! damage to a target.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public ThumperKicks() : base()
        {
            
        }

        public ThumperKicks(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(12, 2, true);
                await MessageHandler.DiceThrow(inst.Location, "12d2!", rolls);
                
                int damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} hits {card.Signature} with a flurry of kicks! {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}