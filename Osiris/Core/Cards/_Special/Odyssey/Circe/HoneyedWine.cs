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
        public override int Cooldown { get; } = 4;

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
                string str = "";
                if(card.Name == "Archer")
                {
                    BasicCard snake = new OSnakeCard(true);
                    str += "Snake";
                    card.CacheAll();
                    card.CopyCard(snake);
                }
                else if(card.Name == "Warrior")
                {
                    str += "Lion";
                    BasicCard lion = new OLionCard(true);
                    card.CacheAll();
                    card.CopyCard(lion);
                }
                else if(card.Name == "Kegmaster")
                {
                    BasicCard pig = new OPigCard(true);
                    str += "Pig";
                    card.CacheAll();
                    card.CopyCard(pig);
                }
                else
                {
                    BasicCard kingfisher = new OKingfisherCard(true);
                    str += "Kingfisher";
                    card.CacheAll();
                    card.CopyCard(kingfisher);
                }

                await MessageHandler.SendMessage(inst.Location, $"{card.Signature} was transformed into a {str} by {inst.GetCardTurn().Signature}.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}