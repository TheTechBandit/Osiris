using System.Collections.Generic;
using System.Threading.Tasks;

namespace Osiris
{
    public class BasicMove
    {
        public virtual string Name { get; }
        public virtual string Owner { get; }
        public virtual string Description { get; }
        //SingleEnemy, AOEEnemy"N", SingleFriendly, AOEFriendly"N", AllEnemy, AllFriendly, All
        public virtual string TargetType { get; }
        public virtual int Targets { get; }
        public virtual bool IsUltimate { get; }
        public virtual int Cooldown { get; }
        public int CurrentCooldown { get; set; }
        public bool OnCooldown { get; set; }

        public BasicMove()
        {

        }

        public BasicMove(bool newcard)
        {
            CurrentCooldown = 0;
            OnCooldown = false;
        }

        public virtual async Task MoveEffect(CombatInstance inst)
        {

        }

        public virtual async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {

        }

        public void CooldownTick()
        {
            CurrentCooldown -= 1;
        }

        public void CooldownReset()
        {
            CurrentCooldown = 0;
        }
    }
}