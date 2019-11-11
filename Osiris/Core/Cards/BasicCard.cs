using System;
using System.Collections.Generic;
using System.Drawing;

namespace Osiris
{
    public class BasicCard
    {
        public virtual string Name { get; }
        public virtual bool RequiresCelestial { get; }
        public virtual List<BasicMove> Moves { get; }
        public ulong Owner { get; set; }
        public string Signature { get; set; }
        public bool Dead { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }
        public bool IsTurn { get; set; }
        public List<BuffDebuff> Effects { get; set; }

        public BasicCard()
        {

        }

        public BasicCard(bool newcard)
        {
            Owner = 0;
            Signature = Name;
            Dead = false;
            TotalHP = 500;
            CurrentHP = 500;
            IsTurn = false;
            Effects = new List<BuffDebuff>();
        }

        public void CooldownTickdown()
        {
            foreach(BasicMove move in Moves)
            {
                move.CooldownTick();
            }
        }

        public void CooldownsReset()
        {
            foreach(BasicMove move in Moves)
            {
                move.CooldownReset();
            }
        }

        public string HPTextString()
        {
            return $"{CurrentHP}/{TotalHP}";
        }

        public List<int> HPGradient()
        {
            double perc = (double)CurrentHP/(double)TotalHP;
            perc *= 100;
            int g = 2*(int)perc;
            int r = 200-g;
            int b = 0;

            return new List<int>()
            {
                r, g, b,
            };
        }

        public BasicMove ParseMove(string str)
        {
            str = str.ToLower();
            foreach(BasicMove move in Moves)
            {
                if(move.Name.ToLower() == str)
                {
                    return move;
                }
            }
            return null;
        }

        public void AddBuff(BuffDebuff buff)
        {
            CurrentHP += buff.Shield;
            TotalHP += buff.TotalShield;
            Effects.Add(buff);
        }

        public int ApplyDamageBuffs(int damage)
        {
            double perc = 0;
            int stat = 0;
            foreach(BuffDebuff eff in Effects)
            {
                perc += eff.DamagePercentBuff;
                perc -= eff.DamagePercentDebuff;
                stat += eff.DamageStaticBuff;
                stat -= eff.DamageStaticDebuff;
                eff.AttackTick();
            }
            EffectCleanup();

            return damage + (int)((double)damage*perc)+stat;
        }

        public void TurnTick()
        {
            foreach(BuffDebuff eff in Effects)
            {
                eff.TurnTick();
            }
            foreach(BasicMove move in Moves)
            {
                move.CooldownTick();
            }
        }

        public void EffectCleanup()
        {
            for (int i = Effects.Count-1; i >= 0; i--)
            {
                if (Effects[i].Attacks == 0 || Effects[i].Turns == 0 || Effects[i].Rounds == 0|| Effects[i].Strikes == 0)
                {
                    CurrentHP -= Effects[i].Shield;
                    TotalHP -= Effects[i].TotalShield;
                    Effects.RemoveAt(i);
                }
            }
        }

        public int TakeDamage(int damage)
        {
            double perc = 0;
            int stat = 0;
            var temp = Int32.MaxValue;
            foreach(BuffDebuff eff in Effects)
            {
                perc += eff.DefensePercentBuff;
                perc -= eff.DefensePercentDebuff;
                stat += eff.DefenseStaticBuff;
                //Find the Minimum of DefenseSetBuff in Effects
                if(eff.DefenseSetBuff >= 0 && eff.DefenseSetBuff < temp)
                    temp = eff.DefenseSetBuff;
                eff.StrikeTick();
            }

            damage = damage - (int)((double)damage*perc)+stat;
            if(damage > temp)
                damage = temp;

            foreach(BuffDebuff eff in Effects)
            {
                if(damage > 0 && eff.Shield > 0)
                {
                    eff.Shield = temp;
                    eff.Shield -= damage;
                    if(eff.Shield < 0)
                        eff.Shield = 0;
                    damage -= temp;
                    if(damage < 0)
                        damage = 0;
                }
            }

            CurrentHP -= damage;
            if(CurrentHP < 0)
                CurrentHP = 0;
            EffectCleanup();
            return damage;
        }

    }
}