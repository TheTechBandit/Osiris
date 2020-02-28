using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class CrushBlind : BasicMove
    {
        public override string Name { get; } = "Crush";
        public override string Owner { get; } = "Polyphemus";
        public override string Description { get; } = "Roll a d4. if the number is 4, Crush the puny humans under your mighty hand. Deal 25d10 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public CrushBlind() : base()
        {
            
        }

        public CrushBlind(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            List<int> hitRoll = RandomGen.RollDice(1, 4);
            await MessageHandler.DiceThrow(inst.Location, "1d4", hitRoll);
            if(hitRoll[0] == 4)
            {
                foreach(BasicCard card in targets)
                {
                    List<int> rolls = RandomGen.RollDice(25, 10);
                    await MessageHandler.DiceThrow(inst.Location, "25d10", rolls);

                    var damage = 0;
                    foreach(int roll in rolls)
                        damage += roll;
                    
                    damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                    var damages = card.TakeDamage(damage);

                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} picks up {card.Signature} and crushes them in his fist! {card.DamageTakenString(damages)}");
                }
            }
            else
            {
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} missed!");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}