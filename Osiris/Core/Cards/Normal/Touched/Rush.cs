using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Rush : BasicMove
    {
        public override string Name { get; } = "Rush";
        public override string Owner { get; } = "Touched";
        public override string Description { get; } = "Plow through the enemy lines! Deal 20 damage to all enemies.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 10;
        public override string CooldownText { get; } = "COOLDOWN: 10 Turns";

        public Rush() : base()
        {
            
        }

        public Rush(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            int damage = 20;
            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            string str = "";
            var totalDam = 0;

            foreach(Team team in inst.Teams)
            {
                if(team.TeamNum != inst.GetTeam(inst.GetCardTurn()).TeamNum)
                {
                    foreach(UserAccount user in team.Members)
                    {
                        foreach(BasicCard card in user.ActiveCards)
                        {
                            var tempDam = card.TakeDamage(damage);
                            totalDam += tempDam[0];
                            str += $"\n{card.DamageTakenString(tempDam)}";
                        }
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} rushes the enemy team!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam}");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}