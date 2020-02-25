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
        //How many targets it hits- SingleEnemy, AOEEnemy"N", SingleFriendly, AOEFriendly"N", AllEnemy, AllFriendly, All, Self
        public virtual string TargetType { get; }
        //Number of targets
        public virtual int Targets { get; }
        //True if this is an ultimate- false if not
        public virtual bool IsUltimate { get; }
        //Number of turns this move takes to cool down
        public virtual int Cooldown { get; }
        //Tells whether or not this move can target dead people
        public bool CanTargetDead { get; set; }
        //Tells whether or not this move can target its owner
        public bool CanTargetSelf { get; set; }
        //Current cooldown timer
        public int CurrentCooldown { get; set; }
        //True if on cooldown, false otherwise
        public bool OnCooldown { get; set; }

        public BasicMove()
        {

        }

        public BasicMove(bool newcard)
        {
            CanTargetDead = false;
            CanTargetSelf = true;
            CurrentCooldown = 0;
            OnCooldown = false;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public virtual async Task MoveEffect(CombatInstance inst)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public virtual async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
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

        public string CooldownText()
        {
            if(Cooldown == 0)
            {
                return "";
            }
            else
            {
                return $"COOLDOWN: {Cooldown} Turns";
            }
        }
    }
}