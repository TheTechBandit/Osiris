using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class EarFlap : BasicMove
    {
        public override string Name { get; } = "Ear Flap";
        public override string Owner { get; } = "Ghub";
        public override string Description { get; } = "Flap your ears with Tremendous force! Roll 4 D20 for damage to the entire enemy team. If you are latched onto a player, deal 50% more damage to that target and dismount.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;
        public override string CooldownText { get; } = "COOLDOWN: 4 Turns";

        public EarFlap() : base()
        {
            
        }

        public EarFlap(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> rolls = RandomGen.RollDice(4, 20);
            string str = "";

            int damage = 0;
            foreach(int roll in rolls)
                damage += roll;

            damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
            var tempDam = 0;

            foreach(Team team in inst.Teams)
            {
                if(team.TeamNum != inst.GetTeam(inst.GetCardTurn()).TeamNum)
                {
                    foreach(UserAccount user in team.Members)
                    {
                        foreach(BasicCard card in user.ActiveCards)
                        {
                            var latchDmg = 1.0;

                            if(card.SearchForMarker(inst.TurnNumber) != null)
                            {
                                latchDmg = 1.5;
                                str = $"\n{inst.GetCardTurn().Signature} dismounts from {card.Signature}, dealing 50% more damage!";
                                for (int i = card.Markers.Count - 1; i >= 0; i--)  
                                {  
                                    if(card.Markers[i].OriginTurnNum == inst.TurnNumber)
                                        card.Markers.RemoveAt(i);
                                }
                            }

                            tempDam = card.TakeDamage((int)(damage*latchDmg));
                            str += $"\n{card.Signature} takes {tempDam} damage!";
                        }
                    }
                }
            }

            await MessageHandler.DiceThrow(inst.Location, "4d20", rolls);
            await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} hits the enemy team with a powerful gust of wind!{str}");

            OnCooldown = true;
            CurrentCooldown = Cooldown;
        }
        
    }
}