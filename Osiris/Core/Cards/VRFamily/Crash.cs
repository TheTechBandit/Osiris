namespace Osiris
{
    public class Crash : BasicMove
    {
        public override string Name { get; } = "Crash";
        public override string Owner { get; } = "VRFamily";
        public override string Description { get; } = "Hit the enemy with a crasher. Roll a D15 for damage to an enemy and flip a coin. If the coin is tails, apply a 15% boost to your next attack.";
        public override string TargetType { get; } = "SingleEnemy";
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Crash() : base()
        {
            
        }

        public Crash(bool newmove) : base(newmove)
        {
            
        }

        public override void MoveEffect()
        {
            
        }
        
    }
}