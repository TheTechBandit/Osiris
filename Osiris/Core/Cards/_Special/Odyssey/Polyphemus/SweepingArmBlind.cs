using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class SweepingArmBlind : BasicMove
    {
        public override string Name { get; } = "Sweeping Arm";
        public override string Owner { get; } = "Polyphemus";
        public override string Description { get; } = "Roll a d4. if the number is 4, Reel your arm across the chamber and wipe out the puny humans. Deal 6d10 to all players and disable them for a turn.";
        public override string TargetType { get; } = "AllEnemy";
        public override int Targets { get; } = 0;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 2;

        public SweepingArmBlind() : base()
        {
            
        }

        public SweepingArmBlind(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst)
        {
            List<int> hitRoll = RandomGen.RollDice(1, 4);
            await MessageHandler.DiceThrow(inst.Location, "1d4", hitRoll);
            if(hitRoll[0] == 4)
            {
                List<int> rolls = RandomGen.RollDice(6, 10);
                await MessageHandler.DiceThrow(inst.Location, "6d10", rolls);
                
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

                                card.AddBuff(new BuffDebuff()
                                {
                                    Name = "Prone",
                                    Origin = $"({inst.GetCardTurn().Signature})",
                                    Description = "disabled.",
                                    TurnSkip = true,
                                    Turns = 1
                                });
                            }
                        }
                    }
                }

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} hits the party with a swing of his arm!{str}\n{inst.GetCardTurn().Signature} dealt a total of {totalDam} damage.");
            }
            else
            {
                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} missed!");
            }
            
            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}