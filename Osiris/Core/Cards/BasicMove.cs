namespace Osiris
{
    public class BasicMove
    {
        public virtual string Name { get; }
        public virtual string Owner { get; }
        public virtual string Description { get; }
        //SingleEnemy, AOEEnemy"N", SingleFriendly, AOEFriendly"N", AllEnemy, AllFriendly, All
        public virtual string TargetType { get; }
        public virtual bool IsUltimate { get; }
        public virtual int Cooldown { get; }
        public int CurrentCooldown { get; set; }

        public BasicMove()
        {

        }

        public BasicMove(bool newcard)
        {
            CurrentCooldown = 0;
        }

        public virtual void MoveEffect()
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