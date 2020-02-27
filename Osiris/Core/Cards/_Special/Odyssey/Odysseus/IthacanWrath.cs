using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class IthacanWrath : BasicMove
    {
        public override string Name { get; } = "Ithacan Wrath";
        public override string Owner { get; } = "Odysseus";
        public override string Description { get; } = "Boost a target allied players' next attack by 50% and heal them for 3d10.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;

        public IthacanWrath() : base()
        {
            
        }

        public IthacanWrath(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(3, 10);
                await MessageHandler.DiceThrow(inst.Location, "3d10", rolls);
                
                int healing = 0;

                foreach(int roll in rolls)
                    healing += roll;

                healing = inst.GetCardTurn().ApplyHealingBuffs(healing, true);
                healing = card.Heal(healing, true);

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Ithacan Wrath",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "50% increased damage on next attack.",
                    DamagePercentBuff = 0.50,
                    Attacks = 1
                });

                await MessageHandler.SendMessage(inst.Location, $"{card.Signature} is instilled with the Wrath of Ithaca! They heal for {healing} and gain 50% increased damage on their next attack.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}