using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ChariotStrike : BasicMove
    {
        public override string Name { get; } = "Chariot Strike";
        public override string Owner { get; } = "Hector";
        public override string Description { get; } = "Ride past the enemy and stab them for 8d10 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public ChariotStrike() : base()
        {
            
        }

        public ChariotStrike(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(8, 10);
                await MessageHandler.DiceThrow(inst.Location, "8d10", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} flies by {card.Signature} in his chariot, taking a swing! {card.DamageTakenString(damages)}");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}