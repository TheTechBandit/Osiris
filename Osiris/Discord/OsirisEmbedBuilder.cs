using System;
using Discord;

namespace Osiris.Discord
{
    public static class OsirisEmbedBuilder
    {
        static OsirisEmbedBuilder()
        {
            
        }

        public static Embed CardList(BasicCard card)
        {
            var builder = new EmbedBuilder()
	        .WithAuthor($"{card.Name} {card.Signature}");

            foreach(BasicMove move in card.Moves)
            {
                builder.AddField($"**{move.Name}**", $"{move.Description}");
            }

            int r = card.HPGradient()[0];
            int g = card.HPGradient()[1];
            int b = card.HPGradient()[2];
        	builder.WithColor(r, g, b);
            var embed = builder.Build();

            return embed;
        }

        public static Embed RoundStart(CombatInstance inst)
        {
            var builder = new EmbedBuilder()
	        .WithAuthor($"Round: {inst.RoundNumber}");

            var players = "";
            foreach(UserAccount user in inst.Players)
            {
                foreach(BasicCard card in user.ActiveCards)
                {
                    players += $"{card.Name} {card.Signature}: {card.CurrentHP}/{card.TotalHP} HP\n";
                }
            }

            builder.WithDescription(players);
            builder.AddField("ACTIVE EFFECTS", ".", false);
            builder.WithFooter($"It is {inst.Players[inst.TurnNumber].Name}'s turn.");
        	builder.WithColor(62, 62, 255);

            var embed = builder.Build();
            return embed;
        }

        public static Embed PlayerTurnStatus(BasicCard card, int round)
        {
            var builder = new EmbedBuilder()
	        .WithAuthor($"{card.Name} {card.Signature}")
            .WithTitle($"HP: {card.CurrentHP}/{card.TotalHP}");

            foreach(BasicMove move in card.Moves)
            {
                builder.AddField($"**{move.Name}**", $"{move.Description}");
            }

            string cooldowns = ".";
            string buffs = ".";
            string debuffs = ".";

            foreach(BasicMove move in card.Moves)
            {
                if(move.OnCooldown)
                    cooldowns += $"{move.Name}- Available on round {move.CurrentCooldown+round}\n";
            }

            builder.AddField("Cooldowns:", cooldowns)
            .AddField("Buffs:", buffs)
            .AddField("Debuffs:", debuffs);

            int r = card.HPGradient()[0];
            int g = card.HPGradient()[1];
            int b = card.HPGradient()[2];
        	builder.WithColor(r, g, b)
            .WithFooter($"Round {round}");
            var embed = builder.Build();

            return embed;
        }

    }
}