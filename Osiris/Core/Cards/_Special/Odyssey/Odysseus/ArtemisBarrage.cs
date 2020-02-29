using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ArtemisBarrage : BasicMove
    {
        public override string Name { get; } = "Artemis' Barrage";
        public override string Owner { get; } = "Odysseus";
        public override string Description { get; } = "Barrage the enemy team with arrows. Deal 6d6 damage to all enemies and flip a coin. if the coin is heads, this ability does not go on cooldown.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public ArtemisBarrage() : base()
        {
            
        }

        public ArtemisBarrage(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(6, 6);
            await MessageHandler.DiceThrow(inst.Location, "6d6", rolls);
            var flip = RandomGen.CoinFlip();
            await MessageHandler.CoinFlip(inst.Location, flip);
            
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

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} calls upon Artemis to barrage the enemy!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");
            
            if(!flip)
            {
                OnCooldown = true;
                CurrentCooldown = Cooldown;
            }
            inst.GetCardTurn().Actions--;
        }
        
    }
}