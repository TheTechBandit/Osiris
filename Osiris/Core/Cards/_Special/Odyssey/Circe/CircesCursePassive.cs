using System.Threading.Tasks;

namespace Osiris
{
    public class CircesCursePassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Circe's Curse";
        //Card this move belongs to
        public override string Owner { get; } = "Lion/Snake/Pig";
        //Description of what this move does
        public override string Description { get; } = "You have been cursed by Circe! You have become a ferocious animal and are now under her command. You cannot pass your turn and you have no choice but to attack your allies and help the enemy. When this form's HP reaches 0, you will return to normal.";
        //Current status of the passive
        public override string Status { get; set; } = ".";

        public CircesCursePassive() : base()
        {

        }

        public CircesCursePassive(bool def) : base(def)
        {
            SetupBuff();
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {

        }
    }
}