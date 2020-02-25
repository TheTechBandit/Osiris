using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class StickyStomp : BasicMove
    {
        public override string Name { get; } = "Sticky Stomp";
        public override string Owner { get; } = "Sugar Ghubby";
        public override string Description { get; } = "Roll 1d20 for damage. Flip a coin. If the coin is heads, roll 1d20. Multiply the initial d20 by the result of this d20. If the result is even, reduce target player's attack damage by 50% on their next attack. If the result is odd, deal this damage to two other random players (if there aren't two other players, stack this damage on the initial target). If the coin is tails, flip another coin. If this coin is heads, shuffle the player turn order and multiply the initial d20 by 4d4! If the second coin is tails, roll 1d10 and plug this number in for x in the function attached. Round down and add this result to the initial 1d20. https://cdn.discordapp.com/attachments/460357767484407809/648324867565027329/unknown.png";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = true;
        public override int Cooldown { get; } = 5;

        public StickyStomp() : base()
        {
            
        }

        public StickyStomp(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> roll1 = RandomGen.RollDice(1, 20);
                await MessageHandler.DiceThrow(inst.Location, "1d20", roll1);
                var flip1 = RandomGen.CoinFlip();
                await MessageHandler.CoinFlip(inst.Location, flip1);

                var damage = roll1[0];
                List<int> tempDams;
                var tempDam = 0;
                var totalDam = 0;
                var hits = 0;
                var shuffled = false;
                List<BasicCard> shuffledCards = new List<BasicCard>();

                //If the first coin flip is heads...
                if(flip1)
                {
                    //Roll another d20 and multiply the first d20 by the second
                    List<int> roll2 = RandomGen.RollDice(1, 20);
                    await MessageHandler.DiceThrow(inst.Location, "1d20", roll2);

                    damage = roll1[0] * roll2[0];
                    await MessageHandler.SendMessage(inst.Location, $"{roll1[0]} * {roll2[0]} = {damage}");

                    tempDam = inst.GetCardTurn().ApplyDamageBuffs(damage);
                    tempDams = card.TakeDamage(tempDam);
                    totalDam += tempDams[0];
                    hits++;
                    await MessageHandler.SendMessage(inst.Location, $"{card.Signature} gets stomped on by {inst.GetCardTurn().Signature}. {card.DamageTakenString(tempDams)}");
                    await Task.Delay(1500);

                    //If the result is even...
                    if(damage % 2 == 0)
                    {
                        card.AddBuff(new BuffDebuff()
                        {
                            Name = "Sticky",
                            Origin = $"({inst.GetCardTurn().Signature})",
                            Description = "Sticky syrup got on you! Your next attack is reduced by 50%",
                            DamagePercentDebuff = 0.5,
                            Attacks = 1
                        });
                    }
                    //If the result is odd...
                    else
                    {
                        var count = 0;
                        foreach(BasicCard card2 in inst.CardList)
                        {
                            if(inst.GetCardTurn().Owner != card2.Owner && card.Owner != card2.Owner)
                            {
                                tempDam = inst.GetCardTurn().ApplyDamageBuffs(damage);
                                tempDams = card2.TakeDamage(tempDam);
                                totalDam += tempDams[0];
                                hits++;
                                await MessageHandler.SendMessage(inst.Location, $"{card2.Signature} gets stomped on by {inst.GetCardTurn().Signature}. {card.DamageTakenString(tempDams)}");
                                await Task.Delay(1500);
                                count++;
                            }

                            if(count == 2)
                                break;
                        }
                        if(count == 0)
                        {
                            tempDam = inst.GetCardTurn().ApplyDamageBuffs(damage);
                            tempDams = card.TakeDamage(tempDam);
                            totalDam += tempDams[0];
                            hits++;
                            await MessageHandler.SendMessage(inst.Location, $"{card.Signature} gets stomped on by {inst.GetCardTurn().Signature} a second time! {card.DamageTakenString(tempDams)}");
                            await Task.Delay(1500);
                            tempDam = inst.GetCardTurn().ApplyDamageBuffs(damage);
                            tempDams = card.TakeDamage(tempDam);
                            totalDam += tempDams[0];
                            hits++;
                            await MessageHandler.SendMessage(inst.Location, $"{card.Signature} gets stomped on by {inst.GetCardTurn().Signature} a third time! {card.DamageTakenString(tempDams)}");
                            await Task.Delay(1500);
                        }
                        if(count == 1)
                        {
                            tempDam = inst.GetCardTurn().ApplyDamageBuffs(damage);
                            tempDams = card.TakeDamage(damage);
                            totalDam += tempDams[0];
                            hits++;
                            await MessageHandler.SendMessage(inst.Location, $"{card.Signature} gets stomped on by {inst.GetCardTurn().Signature} a second time! {card.DamageTakenString(tempDams)}");
                            await Task.Delay(1500);
                        }
                    }
                }
                //If the first coin flip is tails...
                else
                {
                    //Flip another coin
                    var flip2 = RandomGen.CoinFlip();
                    await MessageHandler.CoinFlip(inst.Location, flip2);

                    //If the second coin flip is heads...
                    if(flip2)
                    {
                        List<BasicCard> initialCards = new List<BasicCard>();

                        foreach(BasicCard c in inst.CardList)
                        {
                            initialCards.Add(c);
                        }

                        //Put the current player at the top of the new turn order
                        for(int i = initialCards.Count - 1; i >= 0; i--)  
                        {  
                            if(initialCards[i].Owner == inst.GetCardTurn().Owner)
                            {
                                shuffledCards.Add(initialCards[i]);
                                initialCards.RemoveAt(i);
                            }
                        }

                        int rand = 0;
                        while(initialCards.Count > 0)
                        {
                            rand = RandomGen.RandomNum(0, initialCards.Count-1);
                            shuffledCards.Add(initialCards[rand]);
                            initialCards.RemoveAt(rand);
                        }

                        inst.FixTurnNumber();
                        shuffled = true;
                        
                        List<int> roll3 = RandomGen.RollDice(4, 4, true);
                        await MessageHandler.DiceThrow(inst.Location, "4d4!", roll3);
                        var temp = 0;
                        foreach(int roll in roll3)
                            temp += roll;

                        damage *= temp;
                        await MessageHandler.SendMessage(inst.Location, $"{roll1[0]} * {temp} = {damage}");
                        tempDam = inst.GetCardTurn().ApplyDamageBuffs(damage);
                        tempDams = card.TakeDamage(tempDam);
                        totalDam += tempDams[0];
                        hits++;
                        
                        await MessageHandler.SendMessage(inst.Location, $"{card.Signature} gets stomped on by {inst.GetCardTurn().Signature}. {card.DamageTakenString(tempDams)}");
                        await Task.Delay(1500);
                        await MessageHandler.SendMessage(inst.Location, "The battlefield shakes, shuffling the turn order!");
                        await Task.Delay(1500);
                    }
                    else
                    {
                        List<int> roll4 = RandomGen.RollDice(1, 10);
                        await MessageHandler.DiceThrow(inst.Location, "1d10", roll4);

                        var x = roll4[0];
                        var result = Math.Pow((Math.Abs((Math.Pow(x, 0.5))/(Math.Pow(x, 8))-Math.Log10(x)*x)), 3.14159);

                        damage += (int)result;
                        await MessageHandler.SendMessage(inst.Location, $"{roll1[0]} + {(int)result} = {damage}");

                        tempDam = inst.GetCardTurn().ApplyDamageBuffs(damage);
                        tempDams = card.TakeDamage(tempDam);
                        totalDam += tempDams[0];
                        hits++;
                        
                        await MessageHandler.SendMessage(inst.Location, $"{card.Signature} gets super stomped by {inst.GetCardTurn().Signature}! {card.DamageTakenString(tempDams)}");
                        await Task.Delay(1500);
                    }
                }

                if(hits > 1)
                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");

                if(shuffled)
                    inst.CardList = shuffledCards;
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}