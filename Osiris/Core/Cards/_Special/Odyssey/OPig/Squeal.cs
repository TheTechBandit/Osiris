using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Squeal : BasicMove
    {
        public override string Name { get; } = "Squeal";
        public override string Owner { get; } = "Pig";
        public override string Description { get; } = "Heal Circe and all Animals for 10 HP.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 0;

        public Squeal() : base()
        {
            
        }

        public Squeal(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            int healing = 10;

            healing = inst.GetCardTurn().ApplyHealingBuffs(healing, true);
            string str = "";
            var totalHeal = 0;

            List<BasicCard> targets = inst.GetAOEEnemyTargets();

            foreach(BasicCard card in targets)
            {
                var tempHeal = card.Heal(healing, true);
                totalHeal += tempHeal;
                str += $"\n{card.Signature} heals {tempHeal} HP!";
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} lets out a cute little squeal!{str}\n{inst.GetCardTurn().Signature} healed a total of {totalHeal} health.");
            
            inst.GetCardTurn().Actions--;
        }
        
    }
}