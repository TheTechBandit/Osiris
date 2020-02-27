using System.Threading.Tasks;

namespace Osiris
{
    public class OutcastOfTheGodsPassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Outcast of the Gods";
        //Card this move belongs to
        public override string Owner { get; } = "Odysseus";
        //Description of what this move does
        public override string Description { get; } = "Monsters and nightmares are rapidly attracted to this player.";
        //Current status of the passive
        public override string Status { get; set; } = ".";

        public OutcastOfTheGodsPassive() : base()
        {

        }

        public OutcastOfTheGodsPassive(bool def) : base(def)
        {
            SetupBuff();
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {

        }
    }
}