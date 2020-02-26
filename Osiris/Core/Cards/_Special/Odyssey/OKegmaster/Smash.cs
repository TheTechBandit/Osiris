using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Smash : BasicMove
    {
        public override string Name { get; } = "Smash";
        public override string Owner { get; } = "Kegmaster";
        public override string Description { get; } = "Pummel the enemy with some bare-knuckle-boxing for 2d15 and flip a coin. If the coin is heads, deal 5 extra damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Smash() : base()
        {
            
        }

        public Smash(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(2, 15);
                await MessageHandler.DiceThrow(inst.Location, "2d15", rolls);
                var flip = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;

                if(flip)
                {
                    damage += 5;
                }
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} smashes {card.Signature} with clenched fists! {card.DamageTakenString(damages)}");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}