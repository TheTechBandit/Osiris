using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class StunningPunch : BasicMove
    {
        public override string Name { get; } = "Stunning Punch";
        public override string Owner { get; } = "Warrior";
        public override string Description { get; } = "Reel back a punch that deals 3d10 damage to a target enemy. That enemy deals 15% less damage on their next attack.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public StunningPunch() : base()
        {
            
        }

        public StunningPunch(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(3, 10);
                await MessageHandler.DiceThrow(inst.Location, "3d10", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} punches {card.Signature}! Their next attack is reduced by 15%. {card.DamageTakenString(damages)}");

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Stunned",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "15% less damage on next attack.",
                    DamagePercentDebuff = 0.15,
                    Attacks = 1
                });
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}