using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class EggDrop : BasicMove
    {
        public override string Name { get; } = "Egg Drop";
        public override string Owner { get; } = "Kingfisher";
        public override string Description { get; } = "Incoming! Drop an egg on a target greek. Deal 3d5 damage and decrease their next attack's damage by 15%";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public EggDrop() : base()
        {
            
        }

        public EggDrop(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
            CanTargetEnemies = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(5, 5);
                await MessageHandler.DiceThrow(inst.Location, "5d5", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Egged",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "15% decreased damage.",
                    DamagePercentDebuff = 0.15,
                    Attacks = 1
                });

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} drops an egg on {card.Signature}'s head! {card.DamageTakenString(damages)} Their next attack is reduced by 15%.");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}