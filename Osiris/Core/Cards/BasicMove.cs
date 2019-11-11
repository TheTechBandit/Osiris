using System.Collections.Generic;
using System.Threading.Tasks;

namespace Osiris
{
    public class BasicMove
    {
        //Name of the move
        public virtual string Name { get; }
        //Card this move belongs to
        public virtual string Owner { get; }
        //Description of what this move does
        public virtual string Description { get; }
        //How many targets it hits- SingleEnemy, AOEEnemy"N", SingleFriendly, AOEFriendly"N", AllEnemy, AllFriendly, All
        public virtual string TargetType { get; }
        //Number of targets
        public virtual int Targets { get; }
        //True if this is an ultimate- false if not
        public virtual bool IsUltimate { get; }
        //Number of turns this move takes to cool down
        public virtual int Cooldown { get; }
        //Current cooldown timer
        public int CurrentCooldown { get; set; }
        //True if on cooldown, false otherwise
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
            if(CurrentCooldown > 0)
                CurrentCooldown -= 1;
            if(CurrentCooldown == 0)
                OnCooldown = false;
        }

        public void CooldownReset()
        {
            CurrentCooldown = 0;
        }
    }
}