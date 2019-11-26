using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class SyrupSlide : BasicMove
    {
        public override string Name { get; } = "Syrup Slide";
        public override string Owner { get; } = "Sugar Ghubby";
        public override string Description { get; } = " Engulf all enemy players in syrup! Roll 20d3! for damage to all players. Their next attack is reduced by 50%";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;
        public override string CooldownText { get; } = "Cooldown: 4 Turns";

        public SyrupSlide() : base()
        {
            
        }

        public SyrupSlide(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(20, 3, true);
            int damage = 0;
            foreach(int roll in rolls)
                damage += roll;
            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);

            await MessageHandler.DiceThrow(inst.Location, "20d3!", rolls);

            string str = "";
            var tempDam = 0;
            var totalDam = 0;
            foreach(Team team in inst.Teams)
            {
                if(team.TeamNum != inst.GetTeam(inst.GetCardTurn()).TeamNum)
                {
                    foreach(UserAccount user in team.Members)
                    {
                        foreach(BasicCard card in user.ActiveCards)
                        {
                            tempDam = card.TakeDamage(damage);
                            totalDam += tempDam;
                            str += $"\n{card.Signature} takes {tempDam} damage and has their next attack reduced by 50%";
                            card.AddBuff(new BuffDebuff()
                            {
                                Name = $"Sticky ({inst.GetCardTurn().Signature})",
                                Description = "Sticky syrup got on you! Your next attack is reduced by 50%",
                                DamagePercentDebuff = 0.5,
                                Attacks = 1
                            });
                        }
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} unleashes an avalanche of syrup!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam}");
        }
        
    }
}