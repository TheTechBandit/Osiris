using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Strike : BasicMove
    {
        public override string Name { get; } = "Strike";
        public override string Owner { get; } = "Touched";
        public override string Description { get; } = "Punch the enemy with the force of a soldier. 1 D20 damage to a target enemy.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;
        public override string CooldownText { get; } = "";

        public Strike() : base()
        {
            
        }

        public Strike(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(20);
                int damage = rolls[0];
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                await MessageHandler.DiceThrow(inst.Location, "1d20", rolls);
                damage = card.TakeDamage(damage);
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} punches {card.Signature} with the force of a soldier! {card.Signature} takes {damage} damage!");
            }
        }
        
    }
}