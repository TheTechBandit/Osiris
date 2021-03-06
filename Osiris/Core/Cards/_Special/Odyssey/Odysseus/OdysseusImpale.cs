using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class OdysseusImpale : BasicMove
    {
        public override string Name { get; } = "Impale";
        public override string Owner { get; } = "Odysseus";
        public override string Description { get; } = "Deal 65 damage to a target enemy. That target enemy bleeds 10 damage for 3 turns.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;

        public OdysseusImpale() : base()
        {
            
        }

        public OdysseusImpale(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                var damage = 65;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} imaples {card.Signature}! {card.Signature} is bleeding. {card.DamageTakenString(damages)}");
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Impaled",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "10 bleed damage every turn.",
                    DamagePerTurn = 10,
                    Turns = 3,
                    DPRAlternateText = "bleeding damage."
                });
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}