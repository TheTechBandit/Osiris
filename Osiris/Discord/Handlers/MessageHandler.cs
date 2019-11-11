using System.Collections.Generic;
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

        /* PRESET MESSAGES */
        public static async Task DiceThrow(ContextIds context, string dice, List<int> rolls)
        {
            string str = "";
            int result = 0;
            foreach(int roll in rolls)
            {
                str += $"{roll}, ";
                result += roll;
            }
            str = str.Substring(0, str.Length-2);

            await MessageHandler.SendMessage(context, $"{dice} = ({str}) = {result}");
        }

        public static async Task CoinFlip(ContextIds context, bool flip)
        {
            string str = "Tails";
            if(flip)
            {
                str = "Heads";
            }
            await MessageHandler.SendMessage(context, $"Coin flip! {str}.");
        }

        public static async Task UserIsVictor(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention} wins the duel!");
        }

        public static async Task TeamVictory(ContextIds context, string users, int teamNum)
        {
            await MessageHandler.SendMessage(context, $"Team {teamNum} wins! Congrats to {users}");
        }

        public static async Task TeamEliminated(ContextIds context, int teamNum)
        {
            await MessageHandler.SendMessage(context, $"Team {teamNum} has been eliminated!");
        }

        public static async Task UserForfeitsCombat(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention} has forfeited!");
        }

        public static async Task UserInCombat(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention}, you are in combat!");
        }

        public static async Task UserNotInCombat(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention}, you are not in combat!");
        }

        public static async Task OtherUserInCombat(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention}, that user is in combat!");
        }

        public static async Task OtherUserNotInCombat(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention}, that user is not in combat!");
        }

        public static async Task UserHasNoCards(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, you must have a card to use this command!");
        }

        public static async Task OtherUserHasNoCards(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, that user does not have a card!");
        }

    }
}