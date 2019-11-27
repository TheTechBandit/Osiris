using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class RestingSanctuary : BasicMove
    {
        public override string Name { get; } = "Resting Sanctuary";
        public override string Owner { get; } = "Touched";
        public override string Description { get; } = "Take a break! Heal 3 D10 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;
        public override string CooldownText { get; } = "COOLDOWN: 6 Turns";

        public RestingSanctuary() : base()
        {
            
        }

        public RestingSanctuary(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(3, 10);
            int heal = 0;
            foreach(int roll in rolls)
                heal += roll;
            
            heal = inst.GetCardTurn().ApplyHealingBuffs(heal, true);
            heal = inst.GetCardTurn().Heal(heal, true);
            await MessageHandler.DiceThrow(inst.Location, "3d10", rolls);
            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} takes a moment to rest. They restore {heal} health.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}