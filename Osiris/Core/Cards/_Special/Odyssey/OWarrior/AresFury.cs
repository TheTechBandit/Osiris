using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class AresFury : BasicMove
    {
        public override string Name { get; } = "Ares Fury";
        public override string Owner { get; } = "Warrior";
        public override string Description { get; } = "Praise be the god of war! Your next two attacks are boosted by 30%.";
        public override string TargetType { get; } = "Self";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public AresFury() : base()
        {
            
        }

        public AresFury(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            inst.GetCardTurn().AddBuff(new BuffDebuff()
            {
                Name = "Ares' Fury",
                Buff = true,
                Origin = $"({inst.GetCardTurn().Signature})",
                Description = $"Gain 30% extra damage on your next two attacks.",
                DamagePercentBuff = 0.30,
                Attacks = 2
            });

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} praises Ares! Their next 2 attacks are increased by 30%.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}