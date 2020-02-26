using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class TrojanRoar : BasicMove
    {
        public override string Name { get; } = "Trojan Roar";
        public override string Owner { get; } = "Hector";
        public override string Description { get; } = "All allied players deal an extra 10 damage on their next attack.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public TrojanRoar() : base()
        {
            
        }

        public TrojanRoar(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            foreach(Team team in inst.Teams)
            {
                if(team.TeamNum == inst.GetTeam(inst.GetCardTurn()).TeamNum)
                {
                    foreach(UserAccount user in team.Members)
                    {
                        foreach(BasicCard card in user.ActiveCards)
                        {
                            card.AddBuff(new BuffDebuff()
                            {
                                Name = "Inspired",
                                Origin = $"({inst.GetCardTurn().Signature})",
                                Description = "10 more damage.",
                                DamageStaticBuff = 10,
                                Attacks = 1
                            });
                        }
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} lets out a mighty roar, inspiring their team! All allied players deal 10 extra damage on their next attack.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}