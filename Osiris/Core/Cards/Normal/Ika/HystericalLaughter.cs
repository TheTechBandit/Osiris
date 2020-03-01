using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class HystericalLaughter : BasicMove
    {
        public override string Name { get; } = "Hysterical Laughter";
        public override string Owner { get; } = "Ika";
        public override string Description { get; } = "_The most contagious laughter._ Deal 3 D10 damage to a target enemy and boost the damage of your next attack by 20%";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public HystericalLaughter() : base()
        {
            
        }

        public HystericalLaughter(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(3, 10);
                await MessageHandler.DiceThrow(inst.Location, "3d10", rolls);

                int damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} laughs hysterically at {card.Signature}. {card.DamageTakenString(damages)} {inst.GetCardTurn().Signature} gains a 20% boost on their next attack.");

                inst.GetCardTurn().AddBuff(new BuffDebuff()
                {
                    Name = "Hysterical",
                    Buff = true,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "20% increased damage on next attack.",
                    DamagePercentBuff = 0.2,
                    Attacks = 1
                });

            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}