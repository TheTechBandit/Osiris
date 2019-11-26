using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Gore : BasicMove
    {
        public override string Name { get; } = "Gore";
        public override string Owner { get; } = "Angry Jackalope";
        public override string Description { get; } = "Deal 5d15 damage to a target and flip a coin. If heads, apply a bleeding debuff that deals 5 damage on every attack they make for 2 turns.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;
        public override string CooldownText { get; } = "";

        public Gore() : base()
        {
            
        }

        public Gore(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(5, 15);
                await MessageHandler.DiceThrow(inst.Location, "5d15", rolls);
                var flip = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} gores {card.Signature}! {card.DamageTakenString(damages)}");

                if(flip)
                {
                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} digs into {card.Signature}, causing them to bleed!");
                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Gored",
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "5 bleeding every time an attack is made for 2 turns.",
                        BleedAttackDamage = 5,
                        Turns = 2
                    });
                }
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}