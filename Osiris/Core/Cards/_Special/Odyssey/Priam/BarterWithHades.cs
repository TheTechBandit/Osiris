using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class BarterWithHades : BasicMove
    {
        public override string Name { get; } = "Barter With Hades";
        public override string Owner { get; } = "Priam";
        public override string Description { get; } = "Ressurect up to 2 dead allied players with 1/2 their max HP.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 2;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public BarterWithHades() : base()
        {
            
        }

        public BarterWithHades(bool newmove) : base(newmove)
        {
            CanTargetDead = true;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            string str = "The king is raising the dead!";
            foreach(BasicCard card in targets)
            {
                if(card.Dead)
                {
                    if(inst.GetTeam(card).TeamNum == inst.GetTeam(inst.GetCardTurn()).TeamNum)
                    {
                        card.Dead = false;
                        card.CurrentHP = card.TotalHP/2;

                        str += $"\n{card.Signature} has risen!";
                    }
                    else
                    {
                        await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! You must target players on your team!");
                        return;
                    }
                }
                else
                {
                    await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! One or more players are not dead!");
                    return;
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{str}");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}