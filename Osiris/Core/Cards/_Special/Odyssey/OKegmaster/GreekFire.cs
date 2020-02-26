using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class GreekFire : BasicMove
    {
        public override string Name { get; } = "Greek Fire";
        public override string Owner { get; } = "Kegmaster";
        public override string Description { get; } = "Throw burning oil at the enemy team! All enemies burn for 8 damage each turn for 3 turns.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;

        public GreekFire() : base()
        {
            
        }

        public GreekFire(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            foreach(Team team in inst.Teams)
            {
                if(team.TeamNum != inst.GetTeam(inst.GetCardTurn()).TeamNum)
                {
                    foreach(UserAccount user in team.Members)
                    {
                        foreach(BasicCard card in user.ActiveCards)
                        {
                            card.AddBuff(new BuffDebuff()
                            {
                                Name = "Burning",
                                Origin = $"({inst.GetCardTurn().Signature})",
                                Description = "8 damage each round.",
                                DamagePerRound = 8,
                                DPRAlternateText = "burning damage.",
                                Rounds = 3
                            });
                        }
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} sets the enemy team aflame!");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}