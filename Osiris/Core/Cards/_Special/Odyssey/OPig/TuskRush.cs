using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class TuskRush : BasicMove
    {
        public override string Name { get; } = "Tusk Rush";
        public override string Owner { get; } = "Pig";
        public override string Description { get; } = "Charge into a player with tusks ahead. Deal 4d10 damage. That target bleeds 5 damage on their next turn.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public TuskRush() : base()
        {
            
        }

        public TuskRush(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
            CanTargetEnemies = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(4, 10);
                await MessageHandler.DiceThrow(inst.Location, "4d10", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Bleeding",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "5 damage every turn.",
                    DamagePerTurn = 5,
                    DPRAlternateText = " bleeding damage.",
                    Turns = 1
                });

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} tusk rushes {card.Signature}! {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}