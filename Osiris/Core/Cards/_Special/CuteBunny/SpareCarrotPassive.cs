using System.Threading.Tasks;

namespace Osiris
{
    public class SpareCarrot: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Spare Carrot";
        //Card this move belongs to
        public override string Owner { get; } = "Cute Bunny";
        //Description of what this move does
        public override string Description { get; } = "At the start of each round, a friendly player with the lowest HP will have 10 HP restored.";
        //Current status of the passive
        public override string Status { get; set; } = "";

        public SpareCarrot() : base()
        {

        }

        public SpareCarrot(bool def) : base(def)
        {
            SetupBuff();

            UpdateRoundStart = true;
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {
            BasicCard lowest = null;
            var lowestHP = 101.0;
            foreach(UserAccount teammate in inst.GetTeam(owner).Members)
            {
                foreach(BasicCard card in teammate.ActiveCards)
                {
                    if(card.HPPercentage() < lowestHP && !card.Dead)
                    {
                        lowestHP = card.HPPercentage();
                        lowest = card;
                    }
                }
            }

            if(lowestHP < 100.0 && lowest != null)
            {
                var heal = 10;
                heal = owner.ApplyHealingBuffs(heal, false);
                heal = lowest.Heal(heal, false);
                eff.Extra += heal;
            }
            else
            {
                //Nobody needed healing
            }

            Status = $"Healed a total of {eff.Extra} HP.";
        }
    }
}