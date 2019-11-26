using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class HairRaise : BasicMove
    {
        public override string Name { get; } = "Hair Raise";
        public override string Owner { get; } = "Fluffy Angora";
        public override string Description { get; } = "Make all other party members untargetable and they take 25% less damage for 2 rounds";
        public override string TargetType { get; } = "AllFriendly";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 8;
        public override string CooldownText { get; } = "COOLDOWN: 8 Turns";

        public HairRaise() : base()
        {
            
        }

        public HairRaise(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            foreach(UserAccount user in inst.GetTeam(UserHandler.GetUser(inst.GetCardTurn().Owner)).Members)
            {
                foreach(BasicCard card in user.ActiveCards)
                {
                    if(card.Owner != inst.GetCardTurn().Owner)
                    {
                        BuffDebuff buff = new BuffDebuff()
                        {
                            Name = "Hair Raise",
                            Origin = $"({inst.GetCardTurn().Signature})",
                            Description = "Untargetable and take 25% less damage.",
                            DefensePercentBuff = 0.25,
                            Untargetable = true,
                            Rounds = 2
                        };

                        card.Effects.Add(buff);
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} alerts the party to danger! All other party members are untargetable for 2 rounds and take 25% less damage.");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}