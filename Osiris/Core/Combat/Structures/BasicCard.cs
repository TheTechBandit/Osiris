using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class BasicCard
    {
        public virtual string Name { get; }
        public string CachedName { get; set; }
        public virtual bool RequiresCelestial { get; }
        public virtual bool Hidden { get; }
        public virtual bool Disabled { get; }
        public virtual List<BasicMove> Moves { get; }
        public List<BasicMove> CachedMoves { get; set; }
        public virtual BasicPassive Passive { get; }
        public virtual BasicPassive CachedPassive { get; set; }
        public ulong Owner { get; set; }
        public string Signature { get; set; }
        public string Picture { get; set; }
        public bool HasUltimate { get; set; }
        public bool HasPassive { get; set; }
        public bool Dead { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }
        public int TotalActions { get; set; }
        public int Actions { get; set; }
        public bool IsTurn { get; set; }
        public string DeathMessage { get; set; }
        public List<BuffDebuff> Effects { get; set; }
        public List<Marker> Markers { get; set; }

        public BasicCard()
        {

        }

        public BasicCard(bool newcard)
        {
            Owner = 0;
            Signature = Name;
            Picture = "";
            HasUltimate = true;
            HasPassive = true;
            Dead = false;
            TotalHP = 500;
            CurrentHP = 500;
            TotalActions = 1;
            Actions = TotalActions;
            IsTurn = false;
            DeathMessage = " has been slain!";
            Effects = new List<BuffDebuff>();
            Markers = new List<Marker>();
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

        public double HPPercentage()
        {
            return (double)CurrentHP/(double)TotalHP;
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
            CurrentHP += buff.Growth;
            TotalHP += buff.TotalGrowth;
            Effects.Add(buff);
        }

        public void AddMarker(Marker marker)
        {
            Markers.Add(marker);
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
                if(eff.BleedAttackDamage > 0)
                {
                    TakeDebuffDamage(eff.BleedAttackDamage);
                }
                eff.AttackTick();
            }

            if(HasPassive)
            {
                perc = Passive.PassiveDamagePercentCalculation(perc);
                stat = Passive.PassiveDamageStaticCalculation(stat);
            }
            
            EffectCleanup();

            return damage + (int)((double)damage*perc)+stat;
        }

        public int ApplyHealingBuffs(int heal, bool tick)
        {
            double perc = 0;
            foreach(BuffDebuff eff in Effects)
            {
                perc += eff.HealingPercentBuff;
                perc -= eff.HealingPercentDebuff;
                if(tick)
                    eff.HealTick();
            }
                EffectCleanup();

            return heal + (int)((double)heal*perc);
        }

        public int Heal(int heal, bool tick)
        {
            if(!Dead)
            {
                double perc = 0.0;
                foreach(BuffDebuff eff in Effects)
                {
                    perc += eff.IncomingHealingPercentBuff;
                    perc -= eff.IncomingHealingPercentDebuff;
                    if(tick)
                        eff.IncomingHealTick();
                }
                EffectCleanup();

                heal = heal + (int)((double)heal*perc);

                if(heal < 0)
                    heal = 0;

                var diff = 0;
                var tempHeal = heal;
                foreach(BuffDebuff eff in Effects)
                {
                    if(tempHeal > 0 && eff.TotalGrowth > 0)
                    {
                        diff = eff.TotalGrowth - eff.Growth;

                        tempHeal -= diff;
                        eff.Growth += diff;
                    }
                }

                CurrentHP += heal;
                if(CurrentHP > TotalHP)
                    CurrentHP = TotalHP;

                return heal;
            }
            else
            {
                return 0;
            }
        }

        public async Task RoundTick()
        {
            foreach(BuffDebuff eff in Effects)
            {
                eff.RoundTick();
                if(eff.DamagePerRound > 0)
                {
                    TakeDebuffDamage(eff.DamagePerRound);
                    await MessageHandler.SendMessage(CombatHandler.GetInstance(UserHandler.GetUser(Owner).CombatID).Location, $"{Signature} takes {eff.DamagePerRound} {eff.DPRAlternateText}");
                }
            }
            EffectCleanup();
        }

        public async Task TurnTick()
        {
            foreach(BuffDebuff eff in Effects)
            {
                eff.TurnTick();
                if(eff.DamagePerTurn > 0)
                {
                    TakeDebuffDamage(eff.DamagePerTurn);
                    await MessageHandler.SendMessage(CombatHandler.GetInstance(UserHandler.GetUser(Owner).CombatID).Location, $"{Signature} takes {eff.DamagePerTurn} {eff.DPRAlternateText}");
                }
            }
            foreach(BasicMove move in Moves)
            {
                move.CooldownTick();
            }
            EffectCleanup();
        }

        public void EffectCleanup()
        {
            for (int i = Effects.Count-1; i >= 0; i--)
            {
                if (Effects[i].Attacks == 0 || Effects[i].Turns == 0 || Effects[i].Rounds == 0|| Effects[i].Strikes == 0 || Effects[i].Heals == 0 || Effects[i].IncomingHeals == 0 || (Effects[i].ShieldOnly && Effects[i].LightShield <= 0 && Effects[i].MediumShield <= 0 && Effects[i].HeavyShield <= 0))
                {
                    CurrentHP -= Effects[i].Growth;
                    TotalHP -= Effects[i].TotalGrowth;
                    Effects.RemoveAt(i);
                }
            }
        }

        public List<int> TakeDamage(int damage)
        {
            double perc = 0.0;
            int stat = 0;
            var temp = Int32.MaxValue;
            var heavyBroken = 0;
            var mediumBroken = 0;
            var lightBroken = 0;

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

            if(HasPassive)
            {
                perc = Passive.PassiveDefensePercentCalculation(perc);
                stat = Passive.PassiveDefenseStaticCalculation(stat);
                temp = Passive.PassiveDefenseSetBuffCalculation(temp);
            }

            //Buffs are applied
            damage = damage - (int)((double)damage*perc)+stat;
            if(damage > temp)
                damage = temp;
                
            var totalDamage = damage;

            //Calculate shield cascading
            foreach(BuffDebuff eff in Effects)
            {
                if(eff.LightShield > 0 && damage > 0)
                {
                    while(eff.LightShield > 0 && damage > 0)
                    {
                        damage -= 5;
                        if(damage < 0)
                        {
                            damage = 0;
                        }
                        else
                        {
                            eff.LightShield--;
                            lightBroken++;
                        }
                    }
                }
                if(eff.MediumShield > 0 && TotalLightShield() <= 0 && damage > 0)
                {
                    while(eff.MediumShield > 0 && damage > 0)
                    {
                        damage -= 15;
                        if(damage < 0)
                        {
                            damage = 0;
                        }
                        else
                        {
                            eff.MediumShield--;
                            mediumBroken++;
                        }
                    }
                }
                if(eff.HeavyShield > 0 && TotalLightShield() <= 0 && TotalMediumShield() <= 0 && damage > 0)
                {
                    while(eff.HeavyShield > 0 && damage > 0)
                    {
                        damage -= 30;
                        if(damage < 0)
                        {
                            damage = 0;
                        }
                        else
                        {
                            eff.HeavyShield--;
                            heavyBroken++;
                        }
                    }
                }
            }

            var healthDamage = damage;

            foreach(BuffDebuff eff in Effects)
            {
                if(damage > 0 && eff.Growth > 0)
                {
                    temp = eff.Growth;
                    eff.Growth -= damage;

                    if(eff.Growth < 0)
                        eff.Growth = 0;

                    damage -= temp;

                    if(damage < 0)
                        damage = 0;
                }
            }

            CurrentHP -= healthDamage;

            
            if(CurrentHP < 0)
                CurrentHP = 0;
            
            EffectCleanup();

            List<int> output = new List<int>();
            output.Add(totalDamage);
            output.Add(healthDamage);
            output.Add(heavyBroken);
            output.Add(mediumBroken);
            output.Add(lightBroken);
            return output;
        }

        public int TotalShield()
        {
            var total = 0;
            foreach(BuffDebuff eff in Effects)
            {
                total += eff.HeavyShield;
                total += eff.MediumShield;
                total += eff.LightShield;
            }
            return total;
        }

        public int TotalHeavyShield()
        {
            var total = 0;
            foreach(BuffDebuff eff in Effects)
            {
                total += eff.HeavyShield;
            }
            return total;
        }

        public int TotalMediumShield()
        {
            var total = 0;
            foreach(BuffDebuff eff in Effects)
            {
                total += eff.MediumShield;
            }
            return total;
        }

        public int TotalLightShield()
        {
            var total = 0;
            foreach(BuffDebuff eff in Effects)
            {
                total += eff.LightShield;
            }
            return total;
        }

        public string DamageTakenString(List<int> damages)
        {
            string str = "";
            if(TotalShield() > 0 && damages [2] == 0 && damages[3] == 0 && damages[4] == 0 && damages[1] == 0)
            {
                str += $"{damages[0]} damage bounces off {Signature}'s shields! It wasn't enough to break through.";
            }
            if(damages[2] > 0 || damages[3] > 0 || damages[4] > 0)
            {
                str += $"{Signature} is hit with a total of {damages[0]} damage! {ShieldBreakString(damages)}They take {damages[1]} HP damage.";
            }
            else
            {
                str += $"{Signature} takes {damages[0]} damage!";
            }

            return str;
        }

        public string ShieldBreakString(List<int> damages)
        {
            string str = "";
            if(damages[2] > 0)
            {
                str += $"{damages[2]} heavy shield(s) broke! ";
            }
            if(damages[3] > 0)
            {
                str += $"{damages[3]} medium shield(s) broke! ";
            }
            if(damages[4] > 0)
            {
                str += $"{damages[4]} light shield(s) broke! ";
            }

            return str;
        }

        public void TakeDebuffDamage(int damage)
        {
            var temp = 0;
            var totalDamage = damage;

            foreach(BuffDebuff eff in Effects)
            {
                if(damage > 0 && eff.Growth > 0)
                {
                    temp = eff.Growth;
                    eff.Growth -= damage;

                    if(eff.Growth <= 0)
                        eff.Growth = 0;

                    damage -= temp;

                    if(damage < 0)
                        damage = 0;
                }
            }

            CurrentHP -= totalDamage;

            if(CurrentHP < 0)
                CurrentHP = 0;
        }

        public string GetDeathMessage()
        {
            return $"{Signature}{DeathMessage}";
        }

        //Searches for a mark based on the specified turn number. If found, return this card. IN FUTURE CHANGE THIS TO USE UUID
        public BasicCard SearchForMarker(int turnNum)
        {
            foreach(Marker mark in Markers)
            {
                if(mark.OriginTurnNum == turnNum)
                    return this;
            }

            return null;
        }

        public bool HasBuff(string buffName)
        {
            var hasBuff = false;
            foreach(BuffDebuff eff in Effects)
            {
                if(eff.Name == buffName)
                {
                    hasBuff = true;
                }
            }
            return hasBuff;
        }

        public void RemoveAllBuffs()
        {
            Effects.Clear();
        }

        public void RemoveAllMarkers()
        {
            Markers.Clear();
        }

        public bool IsUntargetable()
        {
            var isUnTarget = false;
            foreach(BuffDebuff eff in Effects)
            {
                if(eff.Untargetable)
                {
                    isUnTarget = true;
                }
            }
            return isUnTarget;
        }

        public bool IsSoleTarget()
        {
            var isSoleTarget = false;
            foreach(BuffDebuff eff in Effects)
            {
                if(eff.SoleTarget)
                {
                    isSoleTarget = true;
                }
            }
            return isSoleTarget;
        }

        public void ApplyBonusActions()
        {
            var bonus = 0;
            
            foreach(BuffDebuff eff in Effects)
            {
                bonus += eff.BonusActions;
            }

            Actions += bonus;
        }

    }
}