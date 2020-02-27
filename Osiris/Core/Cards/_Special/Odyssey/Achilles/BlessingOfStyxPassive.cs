using System.Threading.Tasks;

namespace Osiris
{
    public class BlessingOfStyxPassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Blessing Of Styx";
        //Card this move belongs to
        public override string Owner { get; } = "Achilles";
        //Description of what this move does
        public override string Description { get; } = "Permanently invulnerable.";
        //Current status of the passive
        public override string Status { get; set; } = ".";

        public BlessingOfStyxPassive() : base()
        {

        }

        public BlessingOfStyxPassive(bool def) : base(def)
        {
            SetupBuff();

            UpdateJoinCombat = true;
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {
            eff.DefenseSetBuff = 0;
        }
    }
}