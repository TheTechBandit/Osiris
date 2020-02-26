using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class HealingFlask : BasicMove
    {
        public override string Name { get; } = "Healing Flask";
        public override string Owner { get; } = "Archer";
        public override string Description { get; } = "You or another player may drink from a flask that heals. Heal 20 HP to a target.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public HealingFlask() : base()
        {
            
        }

        public HealingFlask(bool newmove) : base(newmove)
        {

        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                int heal = 20;
                
                heal = inst.GetCardTurn().ApplyHealingBuffs(heal, true);
                heal = card.Heal(heal, true);

                await MessageHandler.SendMessage(inst.Location, $"{card.Signature} drinks from the flask, healing them for {heal} HP.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}