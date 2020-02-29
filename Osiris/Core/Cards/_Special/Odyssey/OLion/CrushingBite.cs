using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class CrushingBite : BasicMove
    {
        public override string Name { get; } = "Crushing Bite";
        public override string Owner { get; } = "Lion";
        public override string Description { get; } = "Gorge your teeth into the greeks! Deal 6d10 damage to a target ally.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public CrushingBite() : base()
        {
            
        }

        public CrushingBite(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
            CanTargetEnemies = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(6, 10);
                await MessageHandler.DiceThrow(inst.Location, "6d10", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} bites down hard on {card.Signature}! {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}