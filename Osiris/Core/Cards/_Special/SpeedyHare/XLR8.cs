using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class XLR8 : BasicMove
    {
        public override string Name { get; } = "XLR8";
        public override string Owner { get; } = "Speedy Hare";
        public override string Description { get; } = "For the next 2 turns, take 2 actions instead of 1.";
        public override string TargetType { get; } = "Self";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;
        public override string CooldownText { get; } = "COOLDOWN: 5 Turns";

        public XLR8() : base()
        {
            
        }

        public XLR8(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            inst.GetCardTurn().AddBuff(new BuffDebuff()
            {
                Name = "XLR8",
                Origin = $"({inst.GetCardTurn().Signature})",
                Description = $"Take 2 actions per turn for your next 2 turns.",
                BonusActions = 1,
                Turns = 3
            });

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} accelerates to insane speeds!");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}