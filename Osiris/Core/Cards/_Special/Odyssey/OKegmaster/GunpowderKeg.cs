using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class GunpowderKeg : BasicMove
    {
        public override string Name { get; } = "Gunpowder Keg";
        public override string Owner { get; } = "Kegmaster";
        public override string Description { get; } = "Hurl an explosive barrel at the enemy team. Deal 4d10 damage to all enemies.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 5;

        public GunpowderKeg() : base()
        {
            
        }

        public GunpowderKeg(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(4, 10);
            await MessageHandler.DiceThrow(inst.Location, "4d10", rolls);
            
            int damage = 0;

            foreach(int roll in rolls)
                damage += roll;

            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            string str = "";
            var totalDam = 0;

            foreach(Team team in inst.Teams)
            {
                if(team.TeamNum != inst.GetTeam(inst.GetCardTurn()).TeamNum)
                {
                    foreach(UserAccount user in team.Members)
                    {
                        foreach(BasicCard card in user.ActiveCards)
                        {
                            var tempDam = card.TakeDamage(damage);
                            totalDam += tempDam[0];
                            str += $"\n{card.DamageTakenString(tempDam)}";
                        }
                    }
                }
            }

            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} tosses an explosive barrel!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}