using System.Collections.Generic;

namespace Osiris
{
    public class BuffDebuff
    {
        public string Name { get; set; } = "Buff";
        public string Origin { get; set; } = "(you should never see this text. if you do, something went wrong and Ghub is an idiot.)";
        public string Description { get; set; } = "A Buff";
        //Rounds this effects last for. -1 for infinite
        public int Rounds { get; set; } = -1;
        //Turns this effect lasts for. -1 for infinite
        public int Turns { get; set; } = -1;
        //Number of attacks this effect lasts for. -1 for infinite
        public int Attacks { get; set; } = -1;
        //Number of hits you can take before the effect wears off. -1 for infinite.
        public int Strikes { get; set; } = -1;
        //Number of heals you can give before the effect wears off. -1 for infinite.
        public int Heals { get; set; } = -1;
        //Number of incoming heals you can receive before the effect ears off. -1 for infinite.
        public int IncomingHeals { get; set; } = -1;
        //If this is Shield Only, it will be removed upon hitting 0 shield
        public bool ShieldOnly { get; set; } = false;
        //Gain x light shielding (blocks up to 5 damage)
        public int LightShield { get; set; } = 0;
        //Gain x light shielding (blocks up to 5 damage)
        public int MediumShield { get; set; } = 0;
        //Gain x light shielding (blocks up to 5 damage)
        public int HeavyShield { get; set; } = 0;
        //Gain x temporary health
        public int TotalGrowth { get; set; } = 0;
        //x temporary health remaining
        public int Growth { get; set; } = 0;
        //Health regained per round
        public int HealingPerRound { get; set; } = 0;
        //Health regained per turn
        public int HealingPerTurn { get; set; } = 0;
        //Damage taken per round
        public int DamagePerRound { get; set; } = 0;
        //Damage taken per turn
        public int DamagePerTurn { get; set; } = 0;
        //Alternate bleed text
        public string DPRAlternateText { get; set; } = "debuff damage.";
        //Bleed damage taken upon using an attack
        public int BleedAttackDamage { get; set; } = 0;
        //healing increased by x%
        public double HealingPercentBuff { get; set; } = 0.0;
        //healing decreased by x%
        public double HealingPercentDebuff { get; set; } = 0.0;
        //Increase healing you receive by x%
        public double IncomingHealingPercentBuff { get; set; } = 0.0;
        //Decrease healing you receive by x%
        public double IncomingHealingPercentDebuff { get; set; } = 0.0;
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
        //Number of bonus actions per turn
        public int BonusActions = 0;
        //If true user's turn will be skipped
        public bool TurnSkip { get; set; } = false;
        //If true, the player will be untargetable (AOE will still hit)
        public bool Untargetable { get; set; } = false;
        //If true, the player is the only valid target (AOE)
        public bool SoleTarget { get; set; } = false;
        //If false, this buff cannot be stacked
        public bool Stackable { get; set; } = true;
        //Extra fields. Used often for Passives. 10 fields by default.
        public List<int> Extra { get; set; } = new List<int>();

        public BuffDebuff()
        {
            SetupExtra();
        }

        public void SetupExtra()
        {
            if(Extra.Count == 0)
            {
                for(int i = 0; i < 10; i++)
                Extra.Add(0);
            }
        }

        public void WipeExtra()
        {
            Extra.Clear();
            SetupExtra();
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

        //Given heals
        public void HealTick()
        {
            if(Heals > 0)
                Heals--;
        }

        //Incoming heals
        public void IncomingHealTick()
        {
            if(IncomingHeals > 0)
                IncomingHeals--;
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
            if(IncomingHeals > 0)
                extra += $" {IncomingHeals} incoming heal(s) remaining.";
            if(LightShield > 0)
                extra += $" {LightShield} light shields(s) remaining.";
            if(MediumShield > 0)
                extra += $" {MediumShield} medium shields(s) remaining.";
            if(HeavyShield > 0)
                extra += $" {HeavyShield} heavy shields(s) remaining.";
            
            return $"**{Name} {Origin}**- {Description}{extra}";
        }

    }
}