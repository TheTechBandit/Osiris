using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class WineBarrel : BasicMove
    {
        public override string Name { get; } = "Wine Barrel";
        public override string Owner { get; } = "Kegmaster";
        public override string Description { get; } = "Douse the battlefield in wine! Heal the ally party for 3d10. All enemy players deal 30% less damage on their next attack.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;

        public WineBarrel() : base()
        {
            
        }

        public WineBarrel(bool newmove) : base(newmove)
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

            List<BasicCard> allyTargets = inst.GetAOEAllyTargets();
            foreach(BasicCard card in allyTargets)
            {
                var tempHeal = card.Heal(healing, true);
                totalHeal += tempHeal;
                str += $"\n{card.Signature} heals {tempHeal} HP!";
            }

            List<BasicCard> targets = inst.GetAOEEnemyTargets();
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Drunkard",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Drunk! Deal 30% less damage on your next attack.",
                    DamagePercentDebuff = 0.30,
                    Attacks = 1
                });
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} douses the battlefield in wine!{str}\n{inst.GetCardTurn().Signature} healed a total of {totalHeal} health. All enemies have their next attack reduced by 30%.");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}