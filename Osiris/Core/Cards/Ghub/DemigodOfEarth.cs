using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class DemigodOfEarth : BasicMove
    {
        public override string Name { get; } = "Demigod Of Earth";
        public override string Owner { get; } = "Ghub";
        public override string Description { get; } = "Gain mass at an alarming rate. You become immune to damage for 2 turns. If you are latched onto an enemy when this is cast, enemy is pinned underneath you for the duration and is disabled for the duration.";
        public override string TargetType { get; } = "Self";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;
        public override string CooldownText { get; } = "COOLDOWN: 5 Turns";

        public DemigodOfEarth() : base()
        {
            
        }

        public DemigodOfEarth(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            string str = "";

            inst.GetCardTurn().AddBuff(new BuffDebuff()
            {
                Name = $"Demigod Of Earth ({inst.GetCardTurn().Signature})",
                Description = "Immune to damage for 2 turns.",
                DefenseSetBuff = 0,
                Turns = 2
            });

            //If any latches are already detected, pin them
            var latches = inst.SearchForMarker(inst.TurnNumber);
            if(latches.Count > 0)
            {
                foreach(BasicCard card in latches)
                {
                    card.AddBuff(new BuffDebuff()
                    {
                        Name = $"Demigod Of Earth ({inst.GetCardTurn().Signature})",
                        Description = "Pinned for 2 turns.",
                        TurnSkip = true,
                        Turns = 2
                    });

                    str += $"\n{card.Signature} is pinned under {inst.GetCardTurn().Signature} and cannot move for 2 turns!";
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} gains a massive amount of mass {str}");
            OnCooldown = true;
            CurrentCooldown = Cooldown;
        }
        
    }
}