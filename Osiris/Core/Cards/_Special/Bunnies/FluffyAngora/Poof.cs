using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Poof : BasicMove
    {
        public override string Name { get; } = "Poof";
        public override string Owner { get; } = "Fluffy Angora";
        public override string Description { get; } = "Poof! Do 4d6 for damage and gain 1d2 light shielding.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Poof() : base()
        {
            
        }

        public Poof(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> damRolls = RandomGen.RollDice(4, 6);
                await MessageHandler.DiceThrow(inst.Location, "4d6", damRolls);
                List<int> shield = RandomGen.RollDice(1, 2);
                await MessageHandler.DiceThrow(inst.Location, "1d2", shield);

                var damage = 0;
                foreach(int roll in damRolls)
                    damage += roll;

                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);

                inst.GetCardTurn().AddBuff(new BuffDebuff()
                {
                    Name = "Poof",
                    Buff = true,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = $"{shield[0]} light shielding.",
                    ShieldOnly = true,
                    LightShield = shield[0]
                });

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} poofs out! They gain {shield[0]} light shield(s) and {card.DamageTakenString(damages)}");
            }

            inst.GetCardTurn().Actions--;
        }
        
    }
}