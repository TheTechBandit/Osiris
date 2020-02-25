using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class StalwartSoul : BasicMove
    {
        public override string Name { get; } = "Stalwart Soul";
        public override string Owner { get; } = "VRFamily";
        public override string Description { get; } = "Nullify all damage to the party for the rest of this turn and the next.";
        public override string TargetType { get; } = "AllFriendly";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 12;

        public StalwartSoul() : base()
        {
            
        }

        public StalwartSoul(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            foreach(UserAccount user in inst.GetTeam(UserHandler.GetUser(inst.GetCardTurn().Owner)).Members)
            {
                foreach(BasicCard card in user.ActiveCards)
                {
                    BuffDebuff buff = new BuffDebuff()
                    {
                        Name = $"Stalwart Soul",
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "All damage nullified.",
                        DefenseSetBuff = 0,
                        Rounds = 2
                    };

                    card.Effects.Add(buff);
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} fortifies the party against all damage!");

            inst.GetCardTurn().Actions--;
            OnCooldown = true;
            CurrentCooldown = Cooldown;
        }
        
    }
}