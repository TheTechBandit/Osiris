using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class MightOfTheGods : BasicMove
    {
        public override string Name { get; } = "Might Of The Gods";
        public override string Owner { get; } = "Polyphemus";
        public override string Description { get; } = "Father give me strength! Your next attack deals X5 damage.";
        public override string TargetType { get; } = "Self";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public MightOfTheGods() : base()
        {
            
        }

        public MightOfTheGods(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            inst.GetCardTurn().AddBuff(new BuffDebuff()
            {
                Name = "Might Of The Gods",
                Origin = $"({inst.GetCardTurn().Signature})",
                Description = $"5X damage on next attack.",
                DamagePercentBuff = 5.00,
                Attacks = 1
            });

            await MessageHandler.SendMessage(inst.Location, $"Poseidon blesses {inst.GetCardTurn().Signature} with the might of the gods! Their next attack deals 5X damage.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}