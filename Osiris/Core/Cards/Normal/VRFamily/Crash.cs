using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Crash : BasicMove
    {
        public override string Name { get; } = "Crash";
        public override string Owner { get; } = "VRFamily";
        public override string Description { get; } = "Hit the enemy with a crasher. Roll a D15 for damage to an enemy and flip a coin. If the coin is tails, apply a 15% boost to your next attack.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Crash() : base()
        {
            
        }

        public Crash(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(15);
                var flip = RandomGen.CoinFlip();

                int damage = rolls[0];
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);
                await MessageHandler.DiceThrow(inst.Location, "1d15", rolls);
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} crashes into {card.Signature}. {card.DamageTakenString(damages)}");
                await MessageHandler.CoinFlip(inst.Location, flip);

                if(!flip)
                {
                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} gains a 15% boost on their next attack.");
                    inst.GetCardTurn().AddBuff(new BuffDebuff()
                    {
                        Name = "Crashed",
                        Buff = true,
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "15% increased damage on next attack.",
                        DamagePercentBuff = 0.15,
                        Attacks = 1
                    });
                }
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}