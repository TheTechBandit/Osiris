using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class StaffBlast : BasicMove
    {
        public override string Name { get; } = "Staff Blast";
        public override string Owner { get; } = "Circe";
        public override string Description { get; } = "Hurl balls of energy at three target enemies. Roll 6d5 for damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 3;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public StaffBlast() : base()
        {
            
        }

        public StaffBlast(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            string str = "";
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(6, 5);
                await MessageHandler.DiceThrow(inst.Location, "6d5", rolls);
                int damage = 0;

                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                str += $"{inst.GetCardTurn().Signature} blasts {card.Signature} with a ball of energy! {card.DamageTakenString(damages)}\n";
            }
            await MessageHandler.SendMessage(inst.Location, $"{str}");

            inst.GetCardTurn().Actions--;
        }
        
    }
}