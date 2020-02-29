using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class HoneyedWine : BasicMove
    {
        public override string Name { get; } = "Honeyed Wine";
        public override string Owner { get; } = "Circe";
        public override string Description { get; } = "Transform up to 2 target enemies into animals to fight for you! ";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 2;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public HoneyedWine() : base()
        {
            
        }

        public HoneyedWine(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                BasicCard pig = new OPigCard(true);
                card.CacheAll();
                card.CopyCard(pig);

                await MessageHandler.SendMessage(inst.Location, $"{card.Signature} is transformed by {inst.GetCardTurn().Signature}.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}