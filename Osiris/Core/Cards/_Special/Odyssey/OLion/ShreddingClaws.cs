using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ShreddingClaws : BasicMove
    {
        public override string Name { get; } = "Shredding Claws";
        public override string Owner { get; } = "Lion";
        public override string Description { get; } = "Lacerate your friends with ferocious claws. Deal 3d10 to all greeks.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public ShreddingClaws() : base()
        {
            
        }

        public ShreddingClaws(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(4, 10);
            await MessageHandler.DiceThrow(inst.Location, "4d10", rolls);
            
            int damage = 0;

            foreach(int roll in rolls)
                damage += roll;

            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            string str = "";
            var totalDam = 0;

            List<BasicCard> targets = inst.GetAOEAllyTargets();

            foreach(BasicCard card in targets)
            {
                var tempDam = card.TakeDamage(damage);
                totalDam += tempDam[0];
                str += $"\n{card.DamageTakenString(tempDam)}";
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} shreds the enemy with their claws!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}