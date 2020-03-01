using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Tweet : BasicMove
    {
        public override string Name { get; } = "Tweet";
        public override string Owner { get; } = "Kingfisher";
        public override string Description { get; } = "Chirp up a storm! All allies heal 2d5 and deal 20% increased damage on their next attack.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Tweet() : base()
        {
            
        }

        public Tweet(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(3, 10);
            await MessageHandler.DiceThrow(inst.Location, "3d10", rolls);
            
            int healing = 0;

            foreach(int roll in rolls)
                healing += roll;

            healing = inst.GetCardTurn().ApplyHealingBuffs(healing, true);
            string str = "";
            var totalHeal = 0;

            List<BasicCard> targets = inst.GetAOEEnemyTargets();

            foreach(BasicCard card in targets)
            {
                var tempHeal = card.Heal(healing, true);
                totalHeal += tempHeal;
                str += $"\n{card.Signature} heals {tempHeal} HP!";

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Birdsong",
                    Buff = true,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "20% increased damage.",
                    DamagePercentBuff = 0.20,
                    Attacks = 1
                });
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} chirps up a storm!{str}\n{inst.GetCardTurn().Signature} healed a total of {totalHeal} health and all allies gain 20% increased damage on their next attack.");
            
            inst.GetCardTurn().Actions--;
        }
        
    }
}