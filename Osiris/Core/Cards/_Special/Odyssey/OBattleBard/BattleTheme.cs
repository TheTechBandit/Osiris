using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class BattleTheme : BasicMove
    {
        public override string Name { get; } = "Battle Theme";
        public override string Owner { get; } = "Battle Bard";
        public override string Description { get; } = "Hype your allies for battle! Choose up to 2 allies. They both gain 1 bonus action on their next turn and have 1 random debuff removed (does not undo transformation).";
        public override string TargetType { get; } = "SingleEnemy";
        public override int Targets { get; } = 2;
        public override bool IsUltimate { get; } = false;
        public override int Cooldown { get; } = 4;

        public BattleTheme() : base()
        {
            
        }

        public BattleTheme(bool newmove) : base(newmove)
        {
            CanTargetSelf = false;
        }

        public override async Task MoveEffect(CombatInstance inst, List<BasicCard> targets)
        {
            foreach(BasicCard card in targets)
            {
                card.AddBuff(new BuffDebuff()
                {
                    Name = "Hyped",
                    Buff = true,
                    Origin = $"({inst.GetCardTurn().Signature})",
                    Description = "1 bonus action.",
                    BonusActions = 1,
                    Turns = 1
                });

                var str = "";
                List<BuffDebuff> debuffs = new List<BuffDebuff>();
                foreach(BuffDebuff buff in card.Effects)
                {
                    if(!buff.Buff)
                        debuffs.Add(buff);
                }

                if(debuffs.Count > 0)
                {
                    BuffDebuff debuffToRemove = debuffs[RandomGen.RandomNum(0, debuffs.Count-1)];
                    str += $" and had their {debuffToRemove.Name} debuff removed.";
                    card.Effects.Remove(debuffToRemove);
                }
                else
                    str += $". They had no debuffs to remove.";

                await MessageHandler.SendMessage(inst.Location, $"{inst.GetCardTurn().Signature} plays a tune for {card.Signature}. They gain 1 bonus action on their next turn{str}");
            }

            OnCooldown = true;
            CurrentCooldown = Cooldown;
            inst.GetCardTurn().Actions--;
        }
        
    }
}