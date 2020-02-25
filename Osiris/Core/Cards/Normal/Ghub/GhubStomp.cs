using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class GhubStomp : BasicMove
    {
        public override string Name { get; } = "Ghub Stomp";
        public override string Owner { get; } = "Ghub";
        public override string Description { get; } = "Plant your foot firmly into the back of a target enemy. Deal 60 DMG. If you are latched onto an enemy, deal an additional 60 DMG. If the enemy is pinned under the effect of Demigod of Earth, Deal an extra 50 DMG.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = true;
        public override int Cooldown { get; } = 18;

        public GhubStomp() : base()
        {
            
        }

        public GhubStomp(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                var damage = 60;


                //If any latches are already detected, add damage
                var latches = inst.SearchForMarker(inst.TurnNumber);
                if(latches.Count > 0)
                {
                    foreach(BasicCard card2 in latches)
                    {
                        if(card2 == card)
                        {
                            damage += 60;

                            foreach(BuffDebuff eff in card2.Effects)
                            {
                                if(eff.Name.Equals($"Demigod Of Earth ({inst.GetCardTurn().Signature})") && eff.Description.Equals("Pinned for 2 turns."))
                                    damage += 50;
                            }
                        }
                    }
                }

                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} Stomps on {card.Signature} with the force of a mountain! {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}