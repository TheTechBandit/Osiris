using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Stab : BasicMove
    {
        public override string Name { get; } = "Stab";
        public override string Owner { get; } = "Trojan Soldier";
        public override string Description { get; } = "Stab the greek invaders! deal 2d5 damage to a target enemy.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Stab() : base()
        {
            
        }

        public Stab(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(2, 5);
                await MessageHandler.DiceThrow(inst.Location, "2d5", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} stabs {card.Signature}! {card.DamageTakenString(damages)}");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}