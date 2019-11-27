using System.Threading.Tasks;

namespace Osiris
{
    public class FluffArmorPassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Fluff Armor";
        //Card this move belongs to
        public override string Owner { get; } = "Fluffy Angora";
        //Description of what this move does
        public override string Description { get; } = "For every combatant in this fight, take 2% less damage.";
        //Current status of the passive
        public override string Status { get; set; } = "ERROR";

        public FluffArmorPassive() : base()
        {

        }

        public FluffArmorPassive(bool def) : base(def)
        {
            SetupBuff();

            UpdatePlayerJoin = true;
            UpdatePlayerLeave = true;
            UpdateJoinCombat = true;
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {
            var playerCount = inst.CardList.Count;
            eff.DefensePercentBuff = 0.50*(double)playerCount;

            Status = $"Currently at **{eff.DefensePercentBuff*100.0}%** less damage.";
        }
    }
}