using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Devour : BasicMove
    {
        public override string Name { get; } = "Devour";
        public override string Owner { get; } = "Sugar Ghubby";
        public override string Description { get; } = "Ingest a target player. They take 20 damage at the start of every round for 3 rounds. They are disabled for this period.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;

        public Devour() : base()
        {
            
        }

        public Devour(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Devoured",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "You have been swallowed! Take 20 damage at the start of every round. You are disabled for this period.",
                    Rounds = 3,
                    DamagePerRound = 20,
                    TurnSkip = true
                });
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} eats {card.Signature} whole!");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}