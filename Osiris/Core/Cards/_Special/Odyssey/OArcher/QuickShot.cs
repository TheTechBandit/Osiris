using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class QuickShot : BasicMove
    {
        public override string Name { get; } = "Quickshot";
        public override string Owner { get; } = "Archer";
        public override string Description { get; } = "Fire an arrow at a target enemy for 3d10 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public QuickShot() : base()
        {
            
        }

        public QuickShot(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                int dice = 3;
                if(inst.GetCardTurn().HasBuff("Firing Position"))
                {
                    dice++;
                }
                List<int> rolls = RandomGen.RollDice(dice, 10);
                await MessageHandler.DiceThrow(inst.Location, $"{dice}d10", rolls);
                int damage = 0;

                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} fires an arrow into {card.Signature}! {card.DamageTakenString(damages)}");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}