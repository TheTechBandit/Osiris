using System;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class NonePassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "None";
        //Card this move belongs to
        public override string Owner { get; } = "None";
        //Description of what this move does
        public override string Description { get; } = "None";
        //Current status of the passive
        public override string Status { get; set; } = "None";

        public NonePassive() : base()
        {

        }

        public NonePassive(bool def) : base(def)
        {
            SetupBuff();
        }
    }
}