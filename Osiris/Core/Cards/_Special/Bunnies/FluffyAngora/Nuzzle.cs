using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Nuzzle : BasicMove
    {
        public override string Name { get; } = "Nuzzle";
        public override string Owner { get; } = "Fluffy Angora";
        public override string Description { get; } = "Give 2 medium shielding to a target. This buff is not stackable.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;

        public Nuzzle() : base()
        {
            
        }

        public Nuzzle(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                var failed = false;
                if(card.HasBuff("Nuzzled"))
                    failed = true;

                if(failed)
                {
                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} lovingly nuzzles {card.Signature}... But they had already been nuzzled! Their affection was denied.");
                    return;
                }
                else
                {
                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Nuzzled",
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = $"{inst.GetCardTurn().Signature}'s nuzzles encourage you! 2 medium shielding. Not stackable.",
                        ShieldOnly = true,
                        MediumShield = 2,
                        Stackable = false
                    });

                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} lovingly nuzzles {card.Signature}. {card.Signature} gains 2 medium shields!");   
                }
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}