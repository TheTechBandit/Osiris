using System.Threading.Tasks;

namespace Osiris
{
    public class SpeederPassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Speeder";
        //Card this move belongs to
        public override string Owner { get; } = "Speedy Hare";
        //Description of what this move does
        public override string Description { get; } = "For every combatant in this fight, your attacks gain 1 damage.";
        //Current status of the passive
        public override string Status { get; set; } = "ERROR";

        public SpeederPassive() : base()
        {

        }

        public SpeederPassive(bool def) : base(def)
        {
            SetupBuff();

            UpdatePlayerJoin = true;
            UpdatePlayerLeave = true;
            UpdateJoinCombat = true;
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {
            var playerCount = inst.CardList.Count;
            eff.DamageStaticBuff = 1*playerCount;

            Status = $"Currently at **{eff.DamageStaticBuff}%** bonus damage.";
        }
    }
}