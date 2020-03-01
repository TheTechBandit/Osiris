using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class CrushingTeeth : BasicMove
    {
        public override string Name { get; } = "Crushing Teeth";
        public override string Owner { get; } = "Charybdis";
        public override string Description { get; } = "Bury your massive jaws into the enemy team. All players receive 40 damage and bleed for 10 damage on their next attack. All players are disabled for a turn.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;

        public CrushingTeeth() : base()
        {
            
        }

        public CrushingTeeth(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            int damage = 40;

            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            string str = "";
            var totalDam = 0;

            List<BasicCard> targets = inst.GetAOEEnemyTargets();

            foreach(BasicCard card in targets)
            {
                var tempDam = card.TakeDamage(damage);
                totalDam += tempDam[0];
                str += $"\n{card.DamageTakenString(tempDam)}";

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Crushed",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Disabled.",
                    TurnSkip = true,
                    BleedAttackDamage = 10,
                    Turns = 1
                });

                card.AddBuff(new BuffDebuff()
                {
                    Name = "Crushed",
                    Buff = false,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Bleeding for 10 damage.",
                    TurnSkip = true,
                    BleedAttackDamage = 10,
                    Attacks = 1
                });
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} crushes the enemy team with sharpened teeth!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage and disabled all enemy players for 1 turn.");
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}