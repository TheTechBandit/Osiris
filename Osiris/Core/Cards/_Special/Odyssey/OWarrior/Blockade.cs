using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Blockade : BasicMove
    {
        public override string Name { get; } = "Blockade";
        public override string Owner { get; } = "Warrior";
        public override string Description { get; } = "Become the sole target of all damage for one turn. Reduce that damage by 75%.";
        public override string TargetType { get; } = "AllFriendly";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;

        public Blockade() : base()
        {
            
        }

        public Blockade(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            inst.GetCardTurn().AddBuff(new BuffDebuff()
            {
                Name = "Blockade",
                Origin = $"({inst.GetCardTurn().Signature})",
                Description = $"Sole Target and damage reduced by 75%",
                DefensePercentBuff = 0.75,
                SoleTarget = true,
                Turns = 2
            });

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} sets up a blockade! They are the sole target and take 75% less damage.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}