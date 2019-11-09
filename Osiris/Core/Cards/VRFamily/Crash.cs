using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Crash : BasicMove
    {
        public override string Name { get; } = "Crash";
        public override string Owner { get; } = "VRFamily";
        public override string Description { get; } = "Hit the enemy with a crasher. Roll a D15 for damage to an enemy and flip a coin. If the coin is tails, apply a 15% boost to your next attack.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Crash() : base()
        {
            
        }

        public Crash(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.CurrentHP -= 50;
                await MessageHandler.SendMessage(inst.Location, $"{Owner} crashes into {card.Name} + {card.Signature}, dealing 50 damage!");
            }
        }
        
    }
}