using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Swallow : BasicMove
    {
        public override string Name { get; } = "Swallow";
        public override string Owner { get; } = "Charybdis";
        public override string Description { get; } = "Swallow an enemy player whole. That player is instantly killed.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 8;

        public Swallow() : base()
        {
            
        }

        public Swallow(bool newmove) : base(newmove)
        {

        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.CurrentHP = 0;
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Name} swallows {card.Signature} whole. They are lost to the deep...");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}