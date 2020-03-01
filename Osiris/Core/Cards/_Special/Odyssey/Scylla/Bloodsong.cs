using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Bloodsong : BasicMove
    {
        public override string Name { get; } = "Bloodsong";
        public override string Owner { get; } = "Scylla";
        public override string Description { get; } = "Let out an ungodly screech. All players are silenced for their next action.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;

        public Bloodsong() : base()
        {
            
        }

        public Bloodsong(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<BasicCard> targets = inst.GetAOEEnemyTargets();
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Ringing Ears",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Silenced.",
                    Silenced = true,
                    Turns = 1
                });
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} lets out an ungodly screech, silencing the enemy team.");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}