using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ArrowOfHermes : BasicMove
    {
        public override string Name { get; } = "Arrow Of Hermes";
        public override string Owner { get; } = "Archer";
        public override string Description { get; } = "Fire an arrow that collateral's into every enemy. Roll 3d15! for damage.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public ArrowOfHermes() : base()
        {
            
        }

        public ArrowOfHermes(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(3, 15, true);
            await MessageHandler.DiceThrow(inst.Location, "3d15!", rolls);
            
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

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} fires a magical arrow!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}