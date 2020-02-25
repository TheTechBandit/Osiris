using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Prance : BasicMove
    {
        public override string Name { get; } = "Prance";
        public override string Owner { get; } = "Speedy Hare";
        public override string Description { get; } = "Deal 4d8 damage to a target and flip two coins. If both are heads, your turn does not end and you may use another ability.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Prance() : base()
        {
            
        }

        public Prance(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(4, 8);
                await MessageHandler.DiceThrow(inst.Location, "4d8", rolls);

                int damage = 0;
                foreach(int roll in rolls)
                    damage += roll;

                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} prances around {card.Signature}. {card.DamageTakenString(damages)}");

                var flip = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip);
                var flip2 = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip2);

                if(flip && flip2)
                {
                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} is hyper! They may use another ability.");
                    inst.GetCardTurn().Actions++;
                }
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}