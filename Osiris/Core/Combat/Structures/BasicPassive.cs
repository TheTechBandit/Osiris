using System;
using System.Threading.Tasks;

namespace Osiris
{
    public class BasicPassive
    {
        //Name of the move
        public virtual string Name { get; }
        //Card this move belongs to
        public virtual string Owner { get; }
        //Description of what this move does
        public virtual string Description { get; }
        //Current status of the passive
        public virtual string Status { get; set; }
        //Buffs involved
        public BuffDebuff eff { get; set; }
        //True if the passive requires Async to run
        public bool RequiresAsync { get; set; } = false;
        //Updates on player joining combat
        public bool UpdatePlayerJoin { get; set; } = false;
        //Updates on player leaving combat
        public bool UpdatePlayerLeave { get; set; } = false;
        //Updates on each round start
        public bool UpdateRoundStart { get; set; } = false;
        //Updates on joining combat
        public bool UpdateJoinCombat { get; set; } = false;


        public BasicPassive()
        {

        }

        public BasicPassive(bool def)
        {
            eff = new BuffDebuff();
        }

        public virtual void Update(CombatInstance inst, BasicCard owner)
        {

        }

        public virtual async Task UpdateAsync(CombatInstance instance, BasicCard owner)
        {
            await Task.Run(null);
        }

        public void SetupBuff()
        {
            eff.Name = Name;
            eff.Origin = "(Passive)";
            eff.Description = Description;
        }

        public double PassiveDamagePercentCalculation(double buff)
        {
            buff += eff.DamagePercentBuff;
            buff -= eff.DamagePercentDebuff;
            return buff;
        }

        public int PassiveDamageStaticCalculation(int buff)
        {
            buff += eff.DamageStaticBuff;
            buff -= eff.DamageStaticDebuff;
            return buff;
        }

        public double PassiveDefensePercentCalculation(double buff)
        {
            buff += eff.DefensePercentBuff;
            buff -= eff.DefensePercentDebuff;
            return buff;
        }

        public int PassiveDefenseStaticCalculation(int buff)
        {
            buff += eff.DefenseStaticBuff;
            return buff;
        }

        public new string ToString()
        {
            return $"**{Name} (Passive)**- {Description}\n{Status}";
        }
    }
}