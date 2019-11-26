using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Chomp : BasicMove
    {
        public override string Name { get; } = "Chomp";
        public override string Owner { get; } = "Ghub";
        public override string Description { get; } = "Bite down on an enemy. Roll 3 D15! for damage and flip a coin. If the coin is heads, you latch on to enemy and cannot be shaken off. **(Chomping again dismounts you from the target)**";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;
        public override string CooldownText { get; } = "";

        public Chomp() : base()
        {
            
        }

        public Chomp(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(3, 15, true);
                var flip = RandomGen.CoinFlip();
                bool latchState = false;

                int damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                //If any latches are already detected, remove them.
                var latches = inst.SearchForMarker(inst.TurnNumber);
                if(latches.Count > 0)
                {
                    latchState = true;
                    foreach(BasicCard card2 in latches)
                    {
                        for (int i = card2.Markers.Count - 1; i >= 0; i--)  
                        {  
                            if(card2.Markers[i].OriginTurnNum == inst.TurnNumber)
                                card2.Markers.RemoveAt(i);
                        }
                    }
                }

                await MessageHandler.DiceThrow(inst.Location, "3d15!", rolls);
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} chomps down on {card.Signature}! {card.DamageTakenString(damages)}");
                await MessageHandler.CoinFlip(inst.Location, flip);

                if(!latchState)
                {
                    if(flip)
                    {
                        await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} is latched on!");
                        card.AddMarker(new Marker(true)
                        {
                            CardOrigin = "Ghub",
                            MoveOrigin = "Chomp",
                            MarkerName = "Latched",
                            OriginTurnNum = inst.TurnNumber,
                            IgnoreBool = false,
                            MarkerBool = true
                        });
                    }
                }
                else
                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} is no longer latched!");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}