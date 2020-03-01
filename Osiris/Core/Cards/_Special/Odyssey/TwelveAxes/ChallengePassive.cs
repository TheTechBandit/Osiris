using System.Threading.Tasks;

namespace Osiris
{
    public class ChallengePassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Challenge";
        //Card this move belongs to
        public override string Owner { get; } = "Twelve Axes";
        //Description of what this move does
        public override string Description { get; } = "Can only be defeated by dealing exacty 12 damage.";
        //Current status of the passive
        public override string Status { get; set; } = ".";

        public ChallengePassive() : base()
        {

        }

        public ChallengePassive(bool def) : base(def)
        {
            SetupBuff();
            eff.TurnSkip = true;
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {
            
        }
    }
}