using System;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class SpareCarrotPassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Spare Carrot";
        //Card this move belongs to
        public override string Owner { get; } = "Cute Bunny";
        //Description of what this move does
        public override string Description { get; } = "At the start of each round, a friendly player with the lowest HP will have 10 HP restored.";
        //Current status of the passive
        public override string Status { get; set; } = ".";

        public SpareCarrotPassive() : base()
        {

        }

        public SpareCarrotPassive(bool def) : base(def)
        {
            SetupBuff();

            RequiresAsync = true;
            UpdateRoundStart = true;
        }

        public override async Task UpdateAsync(CombatInstance inst, BasicCard owner)
        {
            if(inst.RoundNumber != 1)
            {
                BasicCard lowest = null;
                var lowestHP = 1.0;
                foreach(UserAccount teammate in inst.GetTeam(owner).Members)
                {
                    foreach(BasicCard card in teammate.ActiveCards)
                    {
                        if(card.HPPercentage() < lowestHP && !card.Dead && card.Owner != owner.Owner)
                        {
                            Console.WriteLine($"{card.HPPercentage()}% health detected");
                            lowestHP = card.HPPercentage();
                            lowest = card;
                        }
                    }
                }

                if(lowestHP < 1.0 && lowest != null)
                {
                    var heal = 10;
                    heal = owner.ApplyHealingBuffs(heal, false);
                    heal = lowest.Heal(heal, false);
                    eff.Extra[0] += heal;
                    Status = $"Healed a total of {eff.Extra[0]} HP.";

                    await MessageHandler.SendMessage(inst.Location, $"{owner.Signature} gives {lowest.Signature} a spare carrot! They restore {heal} HP.");
                }
                else
                {
                    //Nobody needed healing
                    await MessageHandler.SendMessage(inst.Location, $"{owner.Signature} has a spare carrot but nobody needed healing!");
                }
            }
        }
    }
}