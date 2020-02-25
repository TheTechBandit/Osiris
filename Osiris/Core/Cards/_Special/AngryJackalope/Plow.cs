using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Plow : BasicMove
    {
        public override string Name { get; } = "Plow";
        public override string Owner { get; } = "Angry Jackalope";
        public override string Description { get; } = "Knock the target down. They are disabled on their next turn. Flip a coin. If heads, the target takes 7d10 damage. If tails, the target receives 200% more damage on the next hit they take.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 10;

        public Plow() : base()
        {
            
        }

        public Plow(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} plows through {card.Signature}, knocking them down!");

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Knocked Down",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "You are knocked down! You are disabled on your next turn.",
                    TurnSkip = true,
                    Turns = 1
                });

                var flip = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip);

                if(flip)
                {
                    List<int> rolls = RandomGen.RollDice(7, 10);
                    await MessageHandler.DiceThrow(inst.Location, "7d10", rolls);

                    var damage = 0;
                    foreach(int roll in rolls)
                        damage += roll;
                    
                    damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                    var damages = card.TakeDamage(damage);

                    await MessageHandler.SendMessage(inst.Location, $"{card.Signature} hits the ground hard and takes damage. {card.DamageTakenString(damages)}");
                }
                else
                {
                    await MessageHandler.SendMessage(inst.Location, $"{card.Signature} is vulnerable! They take 200% more damage on the next hit to them.");
                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Vulnerable",
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "You have been knocked down and are vulnerable! You take 200% more damage on the next hit to you.",
                        DefensePercentDebuff = 2.0,
                        Strikes = 1
                    });
                }
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}