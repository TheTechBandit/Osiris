using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Rollout : BasicMove
    {
        public override string Name { get; } = "Rollout";
        public override string Owner { get; } = "Fluffy Angora";
        public override string Description { get; } = "Deal 35 damage to a target. Deal added bonus damage according to the amount of shielding you have (1 light shield = 5 damage)";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public Rollout() : base()
        {
            
        }

        public Rollout(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                var damage = 35;
                var added = 0;
                added += 5 * inst.GetCardTurn().TotalLightShield();
                added += 15 * inst.GetCardTurn().TotalMediumShield();
                added += 30 * inst.GetCardTurn().TotalHeavyShield();

                damage += added;

                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} rolls into {card.Signature}! Their shields provided {added} bonus damage. {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}