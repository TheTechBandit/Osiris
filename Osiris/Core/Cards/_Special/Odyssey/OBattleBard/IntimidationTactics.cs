using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class IndimidationTactics : BasicMove
    {
        public override string Name { get; } = "Indimidation Tactics";
        public override string Owner { get; } = "Battle Bard";
        public override string Description { get; } = "Choose an enemy to intimidate. Their next attack is reduced by 20 damage. Flip a coin. If tails, they are also silenced on their next turn.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;

        public IndimidationTactics() : base()
        {
            
        }

        public IndimidationTactics(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Intimidated",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Damage reduced by 20.",
                    DamageStaticDebuff = 20,
                    Attacks = 1
                });

                var flip = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip);

                var str = "";
                if(!flip)
                {
                    str += " and they were so scared they are Silenced on their next turn!";
                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Terrified",
                        Buff = false,
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "Silenced.",
                        Silenced = true,
                        Turns = 1
                    });
                }
                else
                    str += ".";

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} intimidates {card.Signature}. Their next attack is reduced by 20 damage{str}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}