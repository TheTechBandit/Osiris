using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class DesperateFlurry : BasicMove
    {
        public override string Name { get; } = "Desperate Flurry";
        public override string Owner { get; } = "Touched";
        public override string Description { get; } = "It's make or break! Does 5 D10 damage to a target enemy.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 10;
        public override string CooldownText { get; } = "COOLDOWN: 10 Turns";

        public DesperateFlurry() : base()
        {
            
        }

        public DesperateFlurry(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(5, 10);
                int damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);
                await MessageHandler.DiceThrow(inst.Location, "5d10", rolls);
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} hits {card.Signature} with a flurry of attacks! {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}