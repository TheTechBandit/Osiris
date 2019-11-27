using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Cottontail : BasicMove
    {
        public override string Name { get; } = "Cottontail";
        public override string Owner { get; } = "Cute Bunny";
        public override string Description { get; } = "Shake your tail! Roll 10d7! for damage to a target. Triple this amount and split it evenly between the party as healing.";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 1;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 7;
        public override string CooldownText { get; } = "COOLDOWN: 7 Turns";

        public Cottontail() : base()
        {
            
        }

        public Cottontail(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                List<int> rolls = RandomGen.RollDice(10, 7, true);
                await MessageHandler.DiceThrow(inst.Location, "10d7!", rolls);
                int damage = 0;

                foreach(int roll in rolls)
                    damage += roll;
                
                damage = inst.GetCardTurn().ApplyDamageBuffs(damage);
                var damages = card.TakeDamage(damage);
                var finalDamage = damages[0];

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} shakes their tail at {card.Signature}! Their life was drained from them! {card.DamageTakenString(damages)}");
                var heal = finalDamage * 3;
                heal = inst.GetCardTurn().ApplyHealingBuffs(heal, true);

                var team = inst.GetTeam(inst.GetCardTurn()).Members;
                var rem = heal % team.Count;
                foreach(UserAccount teammate in inst.GetTeam(inst.GetCardTurn()).Members)
                {
                    foreach(BasicCard teamCard in teammate.ActiveCards)
                    {
                        var teamheal = (int)Math.Floor((double)heal/team.Count);
                        if(rem > 0)
                        {
                            rem--;
                            teamheal++;
                        }

                        teamCard.Heal(teamheal, true);
                        await MessageHandler.SendMessage(inst.Location, $"{teamCard.Signature} healed for {teamheal} HP.");
                    }
                }

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn()} healed the party for a total of {heal} HP!");
            }
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}