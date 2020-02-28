using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Regent : BasicMove
    {
        public override string Name { get; } = "Regent";
        public override string Owner { get; } = "Priam";
        public override string Description { get; } = "Select 3 allied players. They become untargetable for one turn and deal 50% more damage for their next attack.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 3;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public Regent() : base()
        {
            
        }

        public Regent(bool newmove) : base(newmove)
        {

        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            string str = "The king is empowering his soldiers!";
            foreach(BasicCard card in targets)
            {
                
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Regent",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "untargetable.",
                    Untargetable = true,
                    Rounds = 1
                });

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Regent",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "50% damage boost",
                    DamagePercentBuff = 0.50,
                    Attacks = 1
                });

                str += $"\n{card.Signature} has become untargetable and deals 50% more damage on their next attack!";
            }

            await MessageHandler.SendMessage(inst.Location, $"{str}");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}