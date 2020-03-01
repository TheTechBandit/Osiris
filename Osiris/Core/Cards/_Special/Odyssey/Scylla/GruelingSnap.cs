using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class GruelingSnap : BasicMove
    {
        public override string Name { get; } = "Grueling Snap";
        public override string Owner { get; } = "Scylla";
        public override string Description { get; } = "Bite down on the humans with vicious scorn. Deal 7d8 damage to a target enemy. That player bleeds for 3 damage for 2 turns.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public GruelingSnap() : base()
        {
            
        }

        public GruelingSnap(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(7, 8);
                await MessageHandler.DiceThrow(inst.Location, "7d8", rolls);
                int damage = 0;

                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Bleeding",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "3 damage every turn.",
                    DamagePerTurn = 3,
                    DPRAlternateText = " bleeding damage.",
                    Turns = 2
                });

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} bites {card.Signature}, causing them to bleed. {card.DamageTakenString(damages)}");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}