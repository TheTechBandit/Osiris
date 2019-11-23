using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Osiris.Discord
{
    public class BasicCommands : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("togglecelestial")]
        public async Task ToggleCelestial(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

            if(user.Celestial == true)
            {
                user.Celestial = false;
                await MessageHandler.SendMessage(idList, $"{user.Name} is no longer **Celestial**!");
            }
            else
            {
                user.Celestial = true;
                await MessageHandler.SendMessage(idList, $"{user.Name} is now **Celestial**!");
            }
        }

        [Command("setcard")]
        public async Task RegisterCard([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var card = CardRegistration.RegisterCard(str);
            var nick = Context.Guild.GetUser(Context.User.Id).Nickname;

            if(nick == null)
                card.Signature = Context.User.Username;
            else
                card.Signature = nick;

            card.Owner = user.UserId;

            if(user.ActiveCards.Count == 0)
                user.ActiveCards.Add(card);
            else
                user.ActiveCards[0] = card;

            await MessageHandler.SendMessage(idList, $"Registered as {user.ActiveCards[0].Name}.");
        }

        [Command("addcardnext")]
        public async Task AddCardNext([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var card = CardRegistration.RegisterCard(str);
            var nick = Context.Guild.GetUser(Context.User.Id).Nickname;

            if(nick == null)
                card.Signature = Context.User.Username;
            else
                card.Signature = nick;

            card.Owner = user.UserId;
            user.ActiveCards.Add(card);

            await MessageHandler.SendMessage(idList, $"Added card {user.ActiveCards[user.ActiveCards.Count-1].Name} to registration.");
        }

        [Command("sigset")]
        public async Task SigSet()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
                await UserHandler.UserHasNoCards(idList, user);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            await MessageHandler.SendMessage(idList, $"Changed signature from {user.ActiveCards[0].Signature} to a blank signature.");

            user.ActiveCards[0].Signature = "";
        }

        [Command("sigset")]
        public async Task SigSet([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
                await UserHandler.UserHasNoCards(idList, user);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            await MessageHandler.SendMessage(idList, $"Changed signature from {user.ActiveCards[0].Signature} to {str}.");

            user.ActiveCards[0].Signature = str;
        }

        [Command("sigset")]
        public async Task SigSet(int i, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
                await UserHandler.UserHasNoCards(idList, user);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            if(i > 0 && i <= user.ActiveCards.Count)
            {
                await MessageHandler.SendMessage(idList, $"Changed signature from {user.ActiveCards[i-1].Signature} to {str}.");

                user.ActiveCards[i-1].Signature = str;
            }
            else
            {
                await MessageHandler.SendMessage(idList, $"The number you choose must be no less than one and no greater than the number of cards you own.");
            }
        }

        [Command("sigset")]
        public async Task SigSet(int i)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
                await UserHandler.UserHasNoCards(idList, user);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            if(i > 0 && i <= user.ActiveCards.Count)
            {
                await MessageHandler.SendMessage(idList, $"Changed signature from {user.ActiveCards[i-1].Signature} to a blank signature.");

                user.ActiveCards[i-1].Signature = "";
            }
            else
            {
                await MessageHandler.SendMessage(idList, $"The number you choose must be no less than one and no greater than the number of cards you own.");
            }
        }

        [Command("removecard")]
        public async Task RemoveCard([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            for(int i = user.ActiveCards.Count-1; i >= 0; i--)
            {
                if(user.ActiveCards[i].Name.ToLower() == str.ToLower())
                {
                    await MessageHandler.SendMessage(idList, $"Removed {user.ActiveCards[i].Name} successfully.");
                    user.ActiveCards.RemoveAt(i);
                }
            }
        }

        [Command("mycards")]
        public async Task ListCards()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            foreach(BasicCard card in user.ActiveCards)
            {
                await MessageHandler.SendEmbedMessage(idList, "", OsirisEmbedBuilder.CardList(card));
            }
        }

        [Command("skillcheck")]
        public async Task CheckCard(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            var other = UserHandler.GetUser(target.Id);

            try
            {
                await UserHandler.OtherUserHasNoCards(idList, user, other);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            if(other.CombatID == -1)
            {
                foreach(BasicCard card in other.ActiveCards)
                {
                    await MessageHandler.SendEmbedMessage(idList, "", OsirisEmbedBuilder.CardList(card));
                }
            }
            else
            {
                foreach(BasicCard card in other.ActiveCards)
                {
                    await MessageHandler.SendEmbedMessage(idList, "", OsirisEmbedBuilder.PlayerTurnStatus(card, CombatHandler.GetInstance(user.CombatID).RoundNumber));
                }
            }
        }

        [Command("skillcheck")]
        public async Task CheckCard([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            await MessageHandler.SendEmbedMessage(idList, "", OsirisEmbedBuilder.CardList(CardRegistration.RegisterCard(str)));
        }

        [Command("echo")]
        public async Task Echo(SocketGuildChannel channel, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            idList.ChannelId = channel.Id;

            await MessageHandler.SendMessage(idList, $"{str}");
        }

    }
}