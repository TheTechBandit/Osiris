using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class VenomSnap : BasicMove
    {
        public override string Name { get; } = "Venom Snap";
        public override string Owner { get; } = "Snake";
        public override string Description { get; } = "Ssssssssss~ Deal 3d10 damage to an ally (excluding animals).";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public VenomSnap() : base()
        {
            
        }

        public VenomSnap(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
            CanTargetEnemies = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(3, 10);
                await MessageHandler.DiceThrow(inst.Location, "3d10", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} venom snaps at {card.Signature}! {card.DamageTakenString(damages)}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}