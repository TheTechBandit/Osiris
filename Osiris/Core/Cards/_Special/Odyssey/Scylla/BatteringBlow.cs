using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class BatteringBlow : BasicMove
    {
        public override string Name { get; } = "Battering Blow";
        public override string Owner { get; } = "Scylla";
        public override string Description { get; } = "Knock the boat around with your enormous body. Deal 5d5 damage to all enemies and disable them for one turn.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 7;

        public BatteringBlow() : base()
        {
            
        }

        public BatteringBlow(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(5, 5);
            await MessageHandler.DiceThrow(inst.Location, "5d5", rolls);
            
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

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Battered",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Disabled.",
                    TurnSkip = true,
                    Turns = 1
                });
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} batters the boat against the cave walls!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage and disabled all enemy players for 1 turn.");
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}