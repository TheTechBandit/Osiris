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
            var builder = new EmbedBuilder();

            if(card.Name.Equals(card.Signature))
                builder.WithAuthor($"{card.Name}");
            else
	            builder.WithAuthor($"{card.Name} {card.Signature}");

            builder.WithImageUrl(card.Picture);

            foreach(BasicMove move in card.Moves)
            {
                string cooldownText = "";
                if(move.Cooldown != 0)
                    cooldownText += $"**{move.CooldownText}**";
                builder.AddField($"**{move.Name}**", $"{move.Description} {cooldownText}");
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
            var effects = "";

            foreach(BasicCard card in inst.CardList)
            {
                if(card.Name.Equals(card.Signature))
                    players += $"[{card.Name}]: {card.CurrentHP}/{card.TotalHP} HP\n";
                else
                    players += $"[{card.Name}] {card.Signature}: {card.CurrentHP}/{card.TotalHP} HPn";
                    
                if(card.Effects.Count > 0 || card.Markers.Count > 0)
                    effects += $"({card.Signature})";
                    
                foreach(BuffDebuff eff in card.Effects)
                {
                    effects += $"\n{eff.ToString()}";
                }
                foreach(Marker mark in card.Markers)
                {
                    effects += $"\n{mark.ToString()}";
                }

                if(card.Effects.Count > 0 || card.Markers.Count > 0)
                    effects += "\n.\n";
            }

            if(effects.Length == 0)
            {
                effects = "none";
            }

            builder.WithDescription(players);
            builder.AddField("ACTIVE EFFECTS", effects, false);
            builder.WithFooter($"It is {inst.Players[inst.TurnNumber].Name}'s turn.");
        	builder.WithColor(62, 62, 255);

            var embed = builder.Build();
            return embed;
        }

        public static Embed PlayerTurnStatus(BasicCard card, int round)
        {
            var builder = new EmbedBuilder();

	        if(card.Name.Equals(card.Signature))
                builder.WithAuthor($"{card.Name}");
            else
	            builder.WithAuthor($"{card.Name} {card.Signature}");

            builder.WithImageUrl(card.Picture);

            builder.WithTitle($"HP: {card.CurrentHP}/{card.TotalHP}\n");

            foreach(BasicMove move in card.Moves)
            {
                string cooldownText = "";
                if(move.Cooldown != 0)
                    cooldownText += $"**{move.CooldownText}**";
                builder.AddField($"**{move.Name}**", $"{move.Description} {cooldownText}");
            }

            string cooldowns = "";
            string effects = "";
            foreach(BuffDebuff eff in card.Effects)
            {
                effects += eff.ToString() + "\n";
            }
            foreach(Marker mark in card.Markers)
            {
                effects += mark.ToString() + "\n";
            }
            if(effects.Length == 0)
                effects += "none";

            foreach(BasicMove move in card.Moves)
            {
                if(move.OnCooldown)
                    cooldowns += $"{move.Name}- Available on round {move.CurrentCooldown+round}\n";
            }
            if(cooldowns.Length == 0)
                cooldowns += "none";

            builder.AddField("Cooldowns:", cooldowns)
            .AddField("Effects:", effects);

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