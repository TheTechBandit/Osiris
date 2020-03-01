using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class FiringPosition : BasicMove
    {
        public override string Name { get; } = "Firing Position";
        public override string Owner { get; } = "Archer";
        public override string Description { get; } = "Take cover from the enemy. Become untargetable for 2 turns and Quick Shot rolls an extra 1d10.";
        public override string TargetType { get; } = "Self";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;

        public FiringPosition() : base()
        {
            
        }

        public FiringPosition(bool newmove) : base(newmove)
        {

        }
        
        public override async Task MoveEffect(CombatInstance inst)
        {
            inst.GetCardTurn().AddBuff(new BuffDebuff()
            {
                Name = "Firing Position",
                Buff = true,
                Origin = $"({inst.GetCardTurn().Signature})",
                Description = "Untargetable",
                Untargetable = true,
                Turns = 3
            });

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} moves to a firing position. They are untargetable for 2 turns.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}