using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Inspiring : BasicMove
    {
        public override string Name { get; } = "Inspiring";
        public override string Owner { get; } = "Priam";
        public override string Description { get; } = "Heal all allied players for 15 HP. Boost their next attack by 10%";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Inspiring() : base()
        {
            
        }

        public Inspiring(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            int healing = 15;

            healing = inst.GetCardTurn().ApplyHealingBuffs(healing, true);
            string str = "";
            var totalHeal = 0;

            List<BasicCard> targets = inst.GetAOEAllyTargets();

            foreach(BasicCard card in targets)
            {
                if(!card.Name.Contains("Priam"))
                {
                    var tempHeal = card.Heal(healing, true);
                    totalHeal += tempHeal;
                    str += $"\n{card.Signature} heals {tempHeal} HP!";

                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Inspired",
                        Buff = true,
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "10% bonus damage",
                        DamagePercentBuff = 0.10,
                        Attacks = 1
                    });
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} inspires his troops!{str}\n{inst.GetCardTurn().Signature} healed a total of {totalHeal} health.");

            inst.GetCardTurn().Actions--;
        }
        
    }
}