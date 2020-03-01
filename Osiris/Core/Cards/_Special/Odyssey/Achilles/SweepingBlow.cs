using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class SweepingBlow : BasicMove
    {
        public override string Name { get; } = "Sweeping Blow";
        public override string Owner { get; } = "Achilles";
        public override string Description { get; } = "Knock down and disable a target player on their next turn. Deal 2d15 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public SweepingBlow() : base()
        {
            
        }

        public SweepingBlow(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(2, 15);
                await MessageHandler.DiceThrow(inst.Location, "2d15", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Prone",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "disabled",
                    TurnSkip = true,
                    Turns = 1
                });

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} sweeps {card.Signature}'s legs out from under them! {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}