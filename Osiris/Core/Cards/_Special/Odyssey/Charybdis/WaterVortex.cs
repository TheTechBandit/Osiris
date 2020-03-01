using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class WaterVortex : BasicMove
    {
        public override string Name { get; } = "WaterVortex";
        public override string Owner { get; } = "Charybdis";
        public override string Description { get; } = "Sweep away the enemy team in a current of water. Deal 4d20 damage to 3d2 targets chosen at random.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public WaterVortex() : base()
        {
            
        }

        public WaterVortex(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(4, 20);
            await MessageHandler.DiceThrow(inst.Location, "4d20", rolls);
            List<int> rolls2 = RandomGen.RollDice(3, 2);
            await MessageHandler.DiceThrow(inst.Location, "3d2", rolls);
            
            int damage = 0;

            foreach(int roll in rolls)
                damage += roll;

            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            string str = "";
            var totalDam = 0;

            var targetCount = 0;
            foreach(int roll in rolls2)
                targetCount += roll;

            List<BasicCard> targets = inst.GetAOERandomRangeEnemyTargets(targetCount);

            foreach(BasicCard card in targets)
            {
                var tempDam = card.TakeDamage(damage);
                totalDam += tempDam[0];
                str += $"\n{card.DamageTakenString(tempDam)}";
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} sweeps away {targetCount} players!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");
            
            inst.GetCardTurn().Actions--;
        }
        
    }
}