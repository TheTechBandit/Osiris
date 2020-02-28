using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class HeelStrike : BasicMove
    {
        public override string Name { get; } = "Heel Strike";
        public override string Owner { get; } = "Priam";
        public override string Description { get; } = "Strike the target's heel. Deal 3d12! damage and disable them for their next turn.";
        public override string TargetType { get; } = "SingleFriendly";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 6;

        public HeelStrike() : base()
        {
            
        }

        public HeelStrike(bool newmove) : base(newmove)
        {
            CanTargetDead = true;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            string str = "";
            foreach(BasicCard card in targets)
            {
                if(card.Name == "Achilles")
                {
                    card.CurrentHP = 0;
                    str += $"{inst.GetCardTurn().Name} strikes Achille's weak point!";
                }
                else
                {
                    List<int> rolls = RandomGen.RollDice(3, 12, true);
                    await MessageHandler.DiceThrow(inst.Location, "3d12", rolls);
                    int damage = 0;

                    foreach(int roll in rolls)
                        damage += roll;
                    
                    damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                    var damages = card.TakeDamage(damage);

                    card.AddBuff(new BuffDebuff()
                    {
                        Name = "Slashed Heel",
                        Origin = $"({inst.GetCardTurn().Signature})",
                        Description = "disabled.",
                        TurnSkip = true,
                        Turns = 1
                    });

                    str = $"{inst.GetCardTurn().Signature} slashes {card.Signature}'s heel! {card.DamageTakenString(damages)}";
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{str}");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}