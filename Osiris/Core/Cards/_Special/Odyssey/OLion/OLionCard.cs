using System;
using System.Collections.Generic;

namespace Osiris
{
    public class OLionCard : BasicCard
    {
        public override string Name { get; set; } = "Lion";
        public override bool RequiresCelestial { get; } = true;
        public override bool Hidden { get; } = true;
        public override bool Disabled { get; } = false;
        public override List<BasicMove> Moves { get; set; } = new List<BasicMove>();
        public override BasicPassive Passive { get; set; } = new CircesCursePassive(true);

        public OLionCard() : base()
        {

        }

        public OLionCard(bool newcard) : base(newcard)
        {
            IsPuppet = true;
            CanPassTurn = false;
            HasUltimate = false;
            
            Picture = "https://media.istockphoto.com/photos/digital-fantasy-art-of-a-lion-picture-id852083940?k=6&m=852083940&s=612x612&w=0&h=QF_uR9BG6nL0dKRlC0ojEq2zu79T5duyl1b8IrFaYTc=";
            Moves.Add(new ShreddingClaws(true));
            Moves.Add(new CrushingBite(true));

            DeathMessage = "'s transformation was reverted!";
            TotalHP = 100;
            CurrentHP = 100;
        }

    }
}