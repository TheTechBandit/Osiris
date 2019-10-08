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
	        .WithAuthor($"{card.Name}");

            foreach(BasicMove move in card.Moves)
            {
                builder.AddField($"**{move.Name}**", $"{move.Description}");
            }

        	builder.WithColor(255, 62, 62);
            var embed = builder.Build();

            return embed;
        }
    }
}