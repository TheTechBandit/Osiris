using System;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public class HerdLeaderPassive: BasicPassive
    {
        //Name of the move
        public override string Name { get; } = "Herd Leader";
        //Card this move belongs to
        public override string Owner { get; } = "Angry Jackalope";
        //Description of what this move does
        public override string Description { get; } = "For each Cute Bunny ally you have a 15% chance of healing an extra 5 HP at the start of the round. For each Fluffy Angora ally you have a 15% chance of gaining 1 light shield at the start of each round. For each Speedy Hare ally you have a 15% chance to gain 1 permanent extra damage at the start of each round.";
        //Current status of the passive
        public override string Status { get; set; } = ".";

        public HerdLeaderPassive() : base()
        {

        }

        public HerdLeaderPassive(bool def) : base(def)
        {
            SetupBuff();

            RequiresAsync = true;
            UpdateRoundStart = true;
        }

        public override async Task UpdateAsync(CombatInstance inst, BasicCard owner)
        {
            if(inst.RoundNumber != 1)
            {
                Console.WriteLine("A");
                var cute = 0;
                var fluffy = 0;
                var speedy = 0;
                foreach(UserAccount teammate in inst.GetTeam(owner).Members)
                {
                    foreach(BasicCard card in teammate.ActiveCards)
                    {
                        if(card.Name.Equals("Cute Bunny"))
                            cute++;
                        if(card.Name.Equals("Fluffy Angora"))
                            fluffy++;
                        if(card.Name.Equals("Speedy Hare"))
                            speedy++;
                    }
                }

                if(cute > 0)
                {
                    var success = RandomGen.PercentChance(15*cute);
                    if(success && owner.CurrentHP < owner.TotalHP)
                    {
                        var heal = 5;
                        heal = owner.ApplyHealingBuffs(heal, false);
                        heal = owner.Heal(heal, false);
                        eff.Extra[0] += heal;
                        Status = $"Healed a total of **{eff.Extra[0]}** HP.\nGained a total of **{eff.Extra[1]}** light shields.\nTotal of **{eff.Extra[2]}** bonus attack damage.";

                        await MessageHandler.SendMessage(inst.Location, $"**(Herd Leader)** {owner.Signature} gained **{heal}** health from **{cute}** Cute Bunnies");
                    }
                }

                if(fluffy > 0)
                {
                    var success = RandomGen.PercentChance(15*fluffy);
                    if(success)
                    {
                        owner.AddBuff(new BuffDebuff()
                        {
                            Name = "Herd Shielding",
                            Buff = true,
                            Origin = $"(Passive)",
                            Description = $"The herd shields you! You gain 1 light shield.",
                            ShieldOnly = true,
                            LightShield = 1
                        });
                        eff.Extra[1]++;
                        Status = $"Healed a total of **{eff.Extra[0]}** HP.\nGained a total of **{eff.Extra[1]}** light shields.\nTotal of **{eff.Extra[2]}** bonus attack damage.";

                        await MessageHandler.SendMessage(inst.Location, $"**(Herd Leader)** {owner.Signature} gained **1 light shield** from **{fluffy}** Fluffy Angora");
                    }
                }

                if(speedy > 0)
                {
                    var success = RandomGen.PercentChance(15*speedy);
                    if(success)
                    {
                        eff.DamageStaticBuff++;
                        eff.Extra[2] = eff.DamageStaticBuff;

                        Status = $"Healed a total of **{eff.Extra[0]}** HP.\nGained a total of **{eff.Extra[1]}** light shields.\nTotal of **{eff.Extra[2]}** bonus attack damage.";

                        await MessageHandler.SendMessage(inst.Location, $"**(Herd Leader)** {owner.Signature} gained **1 permanent bonus damage** from **{speedy}** Fluffy Angora");
                    }
                }

                Console.WriteLine("B");
            }
        }
    }
}