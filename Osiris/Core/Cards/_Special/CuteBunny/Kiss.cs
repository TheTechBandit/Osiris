using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Kiss : BasicMove
    {
        public override string Name { get; } = "Kiss";
        public override string Owner { get; } = "Cute Bunny";
        public override string Description { get; } = "Give a healing kiss! Heal a target for 5d8.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;
        public override string CooldownText { get; } = "";

        public Kiss() : base()
        {
            
        }

        public Kiss(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(5, 8);
                await MessageHandler.DiceThrow(inst.Location, "5d8", rolls);
                int heal = 0;

                foreach(int roll in rolls)
                    heal += roll;
                
                heal = inst.GetCardTurn().ApplyHealingBuffs(heal, true);
                heal = card.Heal(heal, true);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} gives {card.Signature} a little kiss, healing them for {heal} HP.");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}