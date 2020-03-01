using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class HappyDitty : BasicMove
    {
        public override string Name { get; } = "Happy Ditty";
        public override string Owner { get; } = "Battle Bard";
        public override string Description { get; } = "Play an upbeat song! The whole party heals 4d5, gains 1 light shield, deals 20% increased damage on the next attack they do, and takes 10% decreased damage on the next hit they take.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 7;

        public HappyDitty() : base()
        {
            
        }

        public HappyDitty(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(4, 5);
            await MessageHandler.DiceThrow(inst.Location, "4d5", rolls);
            
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

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Lighthearted",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Light Shield.",
                    LightShield = 1,
                    ShieldOnly = true
                });

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Lighthearted",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "20% increased damage.",
                    DamagePercentBuff = 0.20,
                    Attacks = 1
                });

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Lighthearted",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "10% increased defense.",
                    DefensePercentBuff = 0.10,
                    Strikes = 1
                });
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} plays a happy ditty!{str}\n{inst.GetCardTurn().Signature} healed a total of {totalHeal} health. All allies gained various buffs.");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}