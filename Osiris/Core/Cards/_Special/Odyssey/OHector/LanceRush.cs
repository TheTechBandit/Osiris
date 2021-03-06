using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class LanceRush : BasicMove
    {
        public override string Name { get; } = "Lance Rush";
        public override string Owner { get; } = "Hector";
        public override string Description { get; } = "Plow through the enemy lines with your lance, dealing 30 damage to all enemies.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public LanceRush() : base()
        {
            
        }

        public LanceRush(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            int damage = 30;
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

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} rushes the enemy team!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam}");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}