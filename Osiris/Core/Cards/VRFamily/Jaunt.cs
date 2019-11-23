using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Jaunt : BasicMove
    {
        public override string Name { get; } = "Jaunt";
        public override string Owner { get; } = "VRFamily";
        public override string Description { get; } = "Hops through the world at blistering speeds! Deal 45 damage to a target enemy.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;
        public override string CooldownText { get; } = "COOLDOWN: 6 Turns";

        public Jaunt() : base()
        {
            
        }

        public Jaunt(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                int damage = 45;
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                damage = card.TakeDamage(damage);
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} crashes into {card.Signature} with blistering speed! {card.Signature} takes {damage} damage!");
            }
            OnCooldown = true;
            CurrentCooldown = Cooldown;
        }
        
    }
}