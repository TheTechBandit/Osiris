using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class DeRez : BasicMove
    {
        public override string Name { get; } = "De-Rez";
        public override string Owner { get; } = "VRFamily";
        public override string Description { get; } = "Demoralize the enemy! Reduce a target enemy's next 2 attacks by 20% for 2 turns.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 12;
        public override string CooldownText { get; } = "COOLDOWN: 12 Turns";

        public DeRez() : base()
        {
            
        }

        public DeRez(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = $"De-Rez ({inst.GetCardTurn().Signature})",
                    Description = "Attack decreased by 20%.",
                    Attacks = 2,
                    Turns = 2,
                    DamagePercentDebuff = 0.20
                });
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} De-Rezzes {card.Name}, lowering their next 2 attacks by 20% for 2 turns.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
        }
        
    }
}