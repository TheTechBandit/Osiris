using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ShieldBlock : BasicMove
    {
        public override string Name { get; } = "Shield Block";
        public override string Owner { get; } = "Trojan Soldier";
        public override string Description { get; } = "Increase all allied players' defense by 15% for this round.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public ShieldBlock() : base()
        {
            
        }

        public ShieldBlock(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<BasicCard> targets = inst.GetAOEAllyTargets();

            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Shield Block",
                    Buff = true,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "15% less damage.",
                    DefensePercentBuff = 0.15,
                    Rounds = 1
                });
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} raises their shield!");
            
            inst.GetCardTurn().Actions--;
        }
        
    }
}