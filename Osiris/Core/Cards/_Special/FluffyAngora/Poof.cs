using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Poof : BasicMove
    {
        public override string Name { get; } = "Poof";
        public override string Owner { get; } = "Fluffy Angora";
        public override string Description { get; } = "Poof! Deal 4d6 damage to a target and gain a 1d10 shield.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;
        public override string CooldownText { get; } = "";

        public Poof() : base()
        {
            
        }

        public Poof(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> damRolls = RandomGen.RollDice(4, 6);
                await MessageHandler.DiceThrow(inst.Location, "4d6", damRolls);
                List<int> shield = RandomGen.RollDice(1, 10);

                var damage = 0;
                foreach(int roll in damRolls)
                    damage += roll;

                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                damage = card.TakeDamage(damage);

                inst.GetCardTurn().AddBuff(new BuffDebuff()
                    {
                        Name = $"Poof ({inst.GetCardTurn().Signature})",
                        Description = $"{shield} shield.",
                        Growth = 10,
                        TotalGrowth = 10
                    });

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} poofs out! They gain {shield} shield and {card.Signature} takes {damage} damage.");
            }
        }
        
    }
}