using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Swipe : BasicMove
    {
        public override string Name { get; } = "Swipe";
        public override string Owner { get; } = "Sugar Ghubby";
        public override string Description { get; } = "Swipe the enemy team with your giant tail. Deal 25 damage to all enemy players.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Swipe() : base()
        {
            
        }

        public Swipe(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            int damage = 25;
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
                            var tempDams = card.TakeDamage(damage);
                            totalDam += tempDams[0];
                            str += $"\n{card.DamageTakenString(tempDams)}";
                        }
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} swipes using their tail!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");

            inst.GetCardTurn().Actions--;
        }
        
    }
}