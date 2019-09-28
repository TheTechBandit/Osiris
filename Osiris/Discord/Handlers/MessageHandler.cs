using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Osiris.Discord
{
    public static class MessageHandler
    {
        private static DiscordSocketClient _client;

        static MessageHandler()
        {
            _client = Unity.Resolve<DiscordSocketClient>();
        }
        
        public static async Task SendMessage(ulong guildID, ulong channelID, string message)
        {
            await _client.GetGuild(guildID).GetTextChannel(channelID).SendMessageAsync(message);
        }

        public static async Task SendMessage(ContextIds context, string message)
        {
            await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(message);
        }

        public static async Task SendEmbedMessage(ContextIds context, string message, Embed emb)
        {
            await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            message,
            embed: emb)
            .ConfigureAwait(false);
        }

        public static async Task SendDM(ulong userId, string message)
        {
            await _client.GetUser(userId).SendMessageAsync(message);
        }

        public static async Task SendDM(ulong userId, string message, Embed emb)
        {
            await _client.GetUser(userId).SendMessageAsync(
                message,
                embed: emb)
                .ConfigureAwait(false);
        }

    }
}