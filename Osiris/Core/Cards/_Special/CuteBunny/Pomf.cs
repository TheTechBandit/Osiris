using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Pomf : BasicMove
    {
        public override string Name { get; } = "Pomf";
        public override string Owner { get; } = "Cute Bunny";
        public override string Description { get; } = "Pomf! Smack a target for being bad. Deal 2d12 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;
        public override string CooldownText { get; } = "";

        public Pomf() : base()
        {
            
        }

        public Pomf(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(2, 12);
                await MessageHandler.DiceThrow(inst.Location, "2d12", rolls);
                int damage = 0;

                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} smacks {card.Signature} for being mean! {card.DamageTakenString(damages)}");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}