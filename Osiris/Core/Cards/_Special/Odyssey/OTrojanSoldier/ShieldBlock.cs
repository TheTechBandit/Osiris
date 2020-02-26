using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class ShieldBlock : BasicMove
    {
        public override string Name { get; } = "Shield Block";
        public override string Owner { get; } = "Trojan Soldier";
        public override string Description { get; } = "Increase all allied players' defense by 10% on the next hit they recieve.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public ShieldBlock() : base()
        {
            
        }

        public ShieldBlock(bool newmove) : base(newmove)
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
                                Name = "Shield Block",
                                Origin = $"({inst.GetCardTurn().Signature})",
                                Description = "10% less damage.",
                                DefensePercentBuff = 0.10,
                                Strikes = 1
                            });
                        }
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} raises their shield!");
            
            inst.GetCardTurn().Actions--;
        }
        
    }
}