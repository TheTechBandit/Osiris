using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class TorrentFury : BasicMove
    {
        public override string Name { get; } = "Torrent Fury";
        public override string Owner { get; } = "Charybdis";
        public override string Description { get; } = "For every enemy in the fight, deal 5 damage to every enemy.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public TorrentFury() : base()
        {
            
        }

        public TorrentFury(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<BasicCard> targets = inst.GetAOEEnemyTargets();
            int damage = targets.Count*5;

            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            string str = "";
            var totalDam = 0;

            foreach(BasicCard card in targets)
            {
                var tempDam = card.TakeDamage(damage);
                totalDam += tempDam[0];
                str += $"\n{card.DamageTakenString(tempDam)}";
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} starts a torrent.{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}