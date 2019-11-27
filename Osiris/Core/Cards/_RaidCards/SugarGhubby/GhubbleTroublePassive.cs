using System.Threading.Tasks;

namespace Osiris
{
    public class GhubbleTroublePassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Ghubble Trouble";
        //Card this move belongs to
        public override string Owner { get; } = "Sugar Ghubby";
        //Description of what this move does
        public override string Description { get; } = "For every enemy combatant, increase your damage by 1%";
        //Current status of the passive
        public override string Status { get; set; } = "ERROR";

        public GhubbleTroublePassive() : base()
        {

        }

        public GhubbleTroublePassive(bool def) : base(def)
        {
            SetupBuff();

            UpdatePlayerJoin = true;
            UpdatePlayerLeave = true;
            UpdateJoinCombat = true;
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {
            var playerCount = inst.CardList.Count;
            eff.DamagePercentBuff = 0.01*(double)playerCount;

            Status = $"Currently at **{eff.DamagePercentBuff*100.0}%** increased damage.";
        }
    }
}