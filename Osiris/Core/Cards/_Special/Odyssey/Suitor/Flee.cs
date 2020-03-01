using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Flee : BasicMove
    {
        public override string Name { get; } = "Flee";
        public override string Owner { get; } = "Suitor";
        public override string Description { get; } = "Retreat! Become untargetable for a turn.";
        public override string TargetType { get; } = "Self";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public Flee() : base()
        {
            
        }

        public Flee(bool newmove) : base(newmove)
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
                Turns = 2
            });

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} flees into a corner. They are untargetable for 1 turn.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}