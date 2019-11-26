using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Ghubs1911 : BasicMove
    {
        public override string Name { get; } = "Ghub's 1911";
        public override string Owner { get; } = "Ghub";
        public override string Description { get; } = "Empty a clip into a target enemy. Roll 12 D4 For damage. if you are latched onto the target enemy, chomp down harder adding a 10 damage bleed on that enemies next attack.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 3;
        public override string CooldownText { get; } = "COOLDOWN: 3 Turns";

        public Ghubs1911() : base()
        {
            
        }

        public Ghubs1911(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(12, 4);
                bool latchState = false;

                int damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                //If any latches are detected, set latchState to true
                var latches = inst.SearchForMarker(inst.TurnNumber);
                if(latches.Count > 0 && latches.Contains(card))
                {
                    latchState = true;
                }

                await MessageHandler.DiceThrow(inst.Location, "12d4", rolls);
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} unloads a clip into {card.Signature}. {card.DamageTakenString(damages)}");

                if(latchState)
                {
                    await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} chomps down harder, causing {card.Signature} to bleed!");
                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Chomp Wound",
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "receive 10 bleed damage on next attack.",
                        BleedAttackDamage = 10,
                        Attacks = 1
                    });
                }
                
            }
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}