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
        public virtual bool RequiresCelestial { get; }
        public virtual bool Hidden { get; }
        public virtual List<BasicMove> Moves { get; }
        public ulong Owner { get; set; }
        public string Signature { get; set; }
        public string Picture { get; set; }
        public bool Dead { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }
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
            Dead = false;
            TotalHP = 500;
            CurrentHP = 500;
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
            EffectCleanup();

            return damage + (int)((double)damage*perc)+stat;
        }

        public int ApplyHealingBuffs(int heal)
        {
            double perc = 0;
            foreach(BuffDebuff eff in Effects)
            {
                perc += eff.HealingPercentBuff;
                perc -= eff.HealingPercentDebuff;
                eff.HealTick();
            }
            EffectCleanup();

            return heal + (int)((double)heal*perc);
        }

        public async Task RoundTick()
        {
            foreach(BuffDebuff eff in Effects)
            {
                eff.RoundTick();
                if(eff.DamagePerRound > 0)
                {
                    TakeDebuffDamage(eff.DamagePerRound);
                    await MessageHandler.SendMessage(CombatHandler.GetInstance(UserHandler.GetUser(Owner).CombatID).Location, $"{Signature} takes {eff.DamagePerRound} damage from a debuff.");
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
                    await MessageHandler.SendMessage(CombatHandler.GetInstance(UserHandler.GetUser(Owner).CombatID).Location, $"{Signature} takes {eff.DamagePerTurn} damage from a debuff.");
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
                if (Effects[i].Attacks == 0 || Effects[i].Turns == 0 || Effects[i].Rounds == 0|| Effects[i].Strikes == 0 || Effects[i].Heals == 0)
                {
                    CurrentHP -= Effects[i].Growth;
                    TotalHP -= Effects[i].TotalGrowth;
                    Effects.RemoveAt(i);
                }
            }
        }

        public int TakeDamage(int damage)
        {
            double perc = 0;
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

            //Buffs are applied
            damage = damage - (int)((double)damage*perc)+stat;
            if(damage > temp)
                damage = temp;
                
            var totalDamage = damage;

            //Calculate shield cascading
            foreach(BuffDebuff eff in Effects)
            {
                if(damage >= 30 && eff.HeavyShield > 0)
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
                else if(damage >= 15 && eff.MediumShield > 0 && eff.HeavyShield <= 0)
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
                else if(damage >= 5 && eff.LightShield > 0 && eff.MediumShield <= 0 && eff.HeavyShield <= 0)
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
            }

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

            CurrentHP -= damage;

            if(CurrentHP < 0)
                CurrentHP = 0;

            EffectCleanup();
            return damage;
        }

        public void TakeDebuffDamage(int damage)
        {
            var temp = 0;

            foreach(BuffDebuff eff in Effects)
            {
                if(damage > 0 && eff.Growth > 0)
                {
                    eff.Growth = temp;
                    eff.Growth -= damage;
                    if(eff.Growth < 0)
                        eff.Growth = 0;
                    damage -= temp;
                    if(damage < 0)
                        damage = 0;
                }
            }

            CurrentHP -= damage;
        }

        public string GetDeathMessage()
        {
            return $"{Signature}{DeathMessage}";
        }

        //Searches for a mark based on the specified turn number. If found, return this card.
        public BasicCard SearchForMarker(int turnNum)
        {
            foreach(Marker mark in Markers)
            {
                if(mark.OriginTurnNum == turnNum)
                    return this;
            }

            return null;
        }

    }
}