namespace Osiris
{
    public class BuffDebuff
    {
        public string Name { get; set; } = "Buff";
        public string Description { get; set; } = "A Buff";
        //Rounds this effects last for. -1 for infinite
        public int Rounds { get; set; } = -1;
        //Turns this effect lasts for. -1 for infinite
        public int Turns { get; set; } = -1;
        //Number of attacks this effect lasts for. -1 for infinite
        public int Attacks { get; set; } = -1;
        //Number of hits you can take before the effect wears off. -1 for infinite.
        public int Strikes { get; set; } = -1;
        //Number of heals you can get before the effect wears off. -1 for infinite.
        public int Heals { get; set; } = -1;
        //Gain x temporary health
        public int TotalShield { get; set; } = 0;
        //x temporary health remaining
        public int Shield { get; set; } = 0;
        //Health regained per turn
        public int HealingPerTurn { get; set; } = 0;
        //Damage taken per turn
        public int DamagePerTurn { get; set; } = 0;
        //Bleed damage taken upon using an attack
        public int BleedAttackDamage { get; set; } = 0;
        //healing increased by x%
        public double HealingPercentBuff { get; set; } = 0.0;
        //healing decreased by x%
        public double HealingPercentDebuff { get; set; } = 0.0;
        //damage increased by x%
        public double DamagePercentBuff { get; set; } = 0.0;
        //damage increased by x
        public int DamageStaticBuff { get; set; } = 0;
        //Damage reduced by x%
        public double DamagePercentDebuff { get; set; } = 0.0;
        //Damage reduced by x
        public int DamageStaticDebuff { get; set; } = 0;
        //Incoming damage reduced by x%
        public double DefensePercentBuff { get; set; } = 0.0;
        //Incoming damage reduced by x
        public int DefenseStaticBuff { get; set; } = 0;
        //Incoming damage maxed at x. 0 grants immunity, -1 is none
        public int DefenseSetBuff { get; set; } = -1;
        //Incoming damage increased by x%
        public double DefensePercentDebuff { get; set; } = 0.0;
        //If true user's turn will be skipped
        public bool TurnSkip { get; set; } = false;

        public BuffDebuff()
        {

        }

        public int ApplyDamageBuffs(int damage)
        {
            if(DamagePercentBuff > 0)
                damage = damage + (int)((double)damage*DamagePercentBuff);
            damage += DamageStaticBuff;
            AttackTick();
            return damage;
        }
        
        public void RoundTick()
        {
            if(Rounds > 0)
                Rounds--;
        }

        public void TurnTick()
        {
            if(Turns > 0)
                Turns--;
        }

        public void AttackTick()
        {
            if(Attacks > 0)
                Attacks--;
        }

        public void StrikeTick()
        {
            if(Strikes > 0)
                Strikes--;
        }

        public void HealTick()
        {
            if(Heals > 0)
                Heals--;
        }


        public new string ToString()
        {
            string extra = "";
            if(Rounds > 0)
                extra += $" {Rounds} round(s) remaining.";
            if(Turns > 0)
                extra += $" {Turns} turn(s) remaining.";
            if(Attacks > 0)
                extra += $" {Attacks} attack(s) remaining.";
            if(Strikes > 0)
                extra += $" {Strikes} hit(s) remaining.";
            if(Heals > 0)
                extra += $" {Heals} heal(s) remaining.";
            
            return $"**{Name}**- {Description}{extra}";
        }

    }
}