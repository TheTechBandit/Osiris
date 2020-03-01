using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Throw : BasicMove
    {
        public override string Name { get; } = "Throw";
        public override string Owner { get; } = "Suitor";
        public override string Description { get; } = "Throw objects at odysseus' team! Deal 2d5 damage to all enemies.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Throw() : base()
        {
            
        }

        public Throw(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(2, 5);
            await MessageHandler.DiceThrow(inst.Location, "2d5", rolls);
            
            int damage = 0;

            foreach(int roll in rolls)
                damage += roll;

            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            string str = "";
            var totalDam = 0;

            List<BasicCard> targets = inst.GetAOEEnemyTargets();

            foreach(BasicCard card in targets)
            {
                var tempDam = card.TakeDamage(damage);
                totalDam += tempDam[0];
                str += $"\n{card.DamageTakenString(tempDam)}";
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} throws various objects!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");

            inst.GetCardTurn().Actions--;
        }
        
    }
}