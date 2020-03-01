using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ChampionOfTheGods : BasicMove
    {
        public override string Name { get; } = "Champion of the Gods";
        public override string Owner { get; } = "Achilles";
        public override string Description { get; } = "Heal a target player for 80 HP and flip a coin. if the coin is heads, that player is invulnerable for one turn.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public ChampionOfTheGods() : base()
        {
            
        }

        public ChampionOfTheGods(bool newmove) : base(newmove)
        {

        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                int heal = 80;
                var flip = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip);
                
                heal = inst.GetCardTurn().ApplyHealingBuffs(heal, true);
                heal = card.Heal(heal, true);

                string str = "";
                if(flip)
                {
                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Champion's Favor",
                        Buff = true,
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "invulnerable",
                        DefenseSetBuff = 5,
                        Rounds = 2
                    });
                    str += " and are invulnerable for 1 turn";
                }

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} blesses {card.Signature}. They heal for {heal} HP{str}.");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}