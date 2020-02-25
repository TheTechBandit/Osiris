using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class RackTap : BasicMove
    {
        public override string Name { get; } = "Rack Tap";
        public override string Owner { get; } = "Angry Jackalope";
        public override string Description { get; } = "Flip a coin. if the coin is heads, deal 60 damage and flip another. If the next coin is tails, deal an additional 30 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 8;

        public RackTap() : base()
        {
            
        }

        public RackTap(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                var flip = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip);

                var damageFirst = 60;
                var damageSecond = 30;
                var damage = 0;
                string flavorText = "";

                if(flip)
                {
                    damage += damageFirst;
                    flavorText = $"{inst.GetCardTurn().Signature} rams {card.Signature} with their horns!";

                    var flip2 = RandomGen.CoinFlip();
                    await MessageHandler.CoinFlip(inst.Location, flip2);
                    if(!flip2)
                    {
                        damage += damageSecond;
                        flavorText = $"{inst.GetCardTurn().Signature} rams {card.Signature} with their horns!";
                    }
                }

                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{flavorText} {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}