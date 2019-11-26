using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Impale : BasicMove
    {
        public override string Name { get; } = "Impale";
        public override string Owner { get; } = "Angry Jackalope";
        public override string Description { get; } = "Charge into the enemy and impale yourself into them. Deal 40 damage to the target and add a 20% defense debuff to the target that lasts for the next two strikes against them.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;
        public override string CooldownText { get; } = "COOLDOWN: 6 Turns";

        public Impale() : base()
        {
            
        }

        public Impale(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                var damage = 40;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} imaples {card.Signature}! {card.Signature}'s defense is reduced by 20% on the next 2 hits they take. {card.DamageTakenString(damages)}");
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Impaled",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "20% reduced defense on the next 2 hits taken.",
                    DefensePercentDebuff = 0.2,
                    Strikes = 2
                });
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}