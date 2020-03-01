using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class SharpNote : BasicMove
    {
        public override string Name { get; } = "Sharp Note";
        public override string Owner { get; } = "Battle Bard";
        public override string Description { get; } = "Drag your sword across the strings, generating an awful screech! Decrease a target enemy's defense by 15% on the next 2 hits on them and immediately strike them with your sword, dealing 3d8 damage.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public SharpNote() : base()
        {
            
        }

        public SharpNote(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Deafened",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "15% less defense.",
                    DefensePercentDebuff = 0.15,
                    Strikes = 2
                });

                List<int> rolls = RandomGen.RollDice(3, 8);
                await MessageHandler.DiceThrow(inst.Location, "3d8", rolls);

                var damage = 0;
                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} deafens {card.Signature} with a sharp note and strike them with their sword! {card.DamageTakenString(damages)}");
            }
            
            inst.GetCardTurn().Actions--;
        }
        
    }
}