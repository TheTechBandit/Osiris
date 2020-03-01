using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Mending : BasicMove
    {
        public override string Name { get; } = "Mending";
        public override string Owner { get; } = "Circe";
        public override string Description { get; } = "Heal a target ally for 35 HP. Their next attack's damage is increased by 100%.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public Mending() : base()
        {
            
        }

        public Mending(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                int heal = 35;
                
                heal = inst.GetCardTurn().ApplyHealingBuffs(heal, true);
                heal = card.Heal(heal, true);
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Mended",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "100% increased damage.",
                    DamagePercentBuff = 1.00,
                    Attacks = 1
                });

                await MessageHandler.SendMessage(inst.Location, $"{card.Signature} is mended by {inst.GetCardTurn().Signature}. They heal {heal} HP.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}