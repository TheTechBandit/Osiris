using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class Whiskers : BasicMove
    {
        public override string Name { get; } = "Whiskers";
        public override string Owner { get; } = "Cute Bunny";
        public override string Description { get; } = "Target two players. These players get 75% increased damage on their next attack and one random ability that is currently on cooldown will have its cooldown reduced by 2 turns.";
        public override string TargetType { get; } = "AOEFriendly2";
        public override int Targets { get; } = 2;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;
        public override string CooldownText { get; } = "COOLDOWN: 4 Turns";

        public Whiskers() : base()
        {
            
        }

        public Whiskers(bool newmove) : base(newmove)
        {
            
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                string flavorText = $"{inst.GetCardTurn().Signature} buffs {card.Signature} increasing their next attack by 75%.";
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Whiskers",
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "Next attack's damage increased by 75%",
                    Attacks = 1,
                    DamagePercentBuff = 0.75
                });

                List<BasicMove> moves = new List<BasicMove>();
                foreach(BasicMove move in card.Moves)
                {
                    if(move.CurrentCooldown > 0 && move.OnCooldown)
                        moves.Add(move);
                }

                if(moves.Count == 0)
                {
                    await MessageHandler.SendMessage(inst.Location, $"{flavorText} They didn't have any moves on cooldown.");
                }
                else
                {
                    var rand = RandomGen.RandomNum(0, moves.Count-1);
                    moves[rand].CurrentCooldown -= 2;
                    if(moves[rand].CurrentCooldown <= 0)
                    {
                        moves[rand].OnCooldown = false;
                        moves[rand].CurrentCooldown = 0;

                        await MessageHandler.SendMessage(inst.Location, $"{flavorText} {moves[rand].Name}'s cooldown was reduced by 2.");
                    }
                }
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}