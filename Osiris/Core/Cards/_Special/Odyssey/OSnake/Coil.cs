using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Coil : BasicMove
    {
        public override string Name { get; } = "Coil";
        public override string Owner { get; } = "Snake";
        public override string Description { get; } = "Coil around an animal friend and reduce the next damage they take by 30%.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public Coil() : base()
        {
            
        }

        public Coil(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
            CanTargetAllies = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Coiled Friend",
                    Buff = true,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "30% reduced damage.",
                    DefensePercentBuff = 0.30,
                    Strikes = 1
                });

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} coils around {card.Signature}, reducing the next hit they take by 30%.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}