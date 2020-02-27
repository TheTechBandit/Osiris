using System.Threading.Tasks;

namespace Osiris
{
    public class WarriorOfAthenaPassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Warrior Of Athena";
        //Card this move belongs to
        public override string Owner { get; } = "Odysseus";
        //Description of what this move does
        public override string Description { get; } = "Every ally in the fight boosts odysseus' damage by 5%.";
        //Current status of the passive
        public override string Status { get; set; } = "ERROR";

        public WarriorOfAthenaPassive() : base()
        {

        }

        public WarriorOfAthenaPassive(bool def) : base(def)
        {
            SetupBuff();

            UpdatePlayerJoin = true;
            UpdatePlayerLeave = true;
            UpdateJoinCombat = true;
        }

        public override void Update(CombatInstance inst, BasicCard owner)
        {
            var playerCount = -1;
            foreach(UserAccount user in inst.GetTeam(inst.GetCardTurn()).Members)
            {
                foreach(BasicCard allyCard in user.ActiveCards)
                {
                    playerCount++;
                }
            }

            eff.DamagePercentBuff = 0.05*playerCount;

            Status = $"Currently at **{eff.DamagePercentBuff*100.0}%** bonus damage.";
        }
    }
}