using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Reel : BasicMove
    {
        public override string Name { get; } = "Reel";
        public override string Owner { get; } = "Speedy Hare";
        public override string Description { get; } = "Roll 1d100. Your next attack is increased by the percent rolled.";
        public override string TargetType { get; } = "Self";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Reel() : base()
        {
            
        }

        public Reel(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(1, 100);
            await MessageHandler.DiceThrow(inst.Location, "1d100", rolls);

            double perc = (double)rolls[0]/100.0;

            inst.GetCardTurn().AddBuff(new BuffDebuff()
            {
                Name = "Reel",
                Buff = true,
                Origin = $"({inst.GetCardTurn().Signature})",
                Description = $"Gain {rolls[0]}% extra damage on your next attack.",
                DamagePercentBuff = perc,
                Attacks = 1
            });

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} prepares their next attack, gaining a {rolls[0]}% buff to their next attack!");

            inst.GetCardTurn().Actions--;
        }
        
    }
}