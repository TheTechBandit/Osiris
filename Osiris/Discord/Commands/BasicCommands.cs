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

        [Command("addcard")]
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

            string[] split = str.Split(' ');

            if(split.Length > 2)
            {
                await MessageHandler.SendMessage(idList, $"You can't use more than 2 parameters. Example Use: \"addcard vrfamily (Osiris)\"");
            }
            else
            {
                if(user.ActiveCards.Count == 0)
                {
                    var card = CardRegistration.RegisterCard(str);
                    if(split.Length == 2)
                        card.Signature = split[1];
                    card.Owner = user.UserId;
                    user.ActiveCards.Add(card);
                }
                else
                {
                    var card = CardRegistration.RegisterCard(str);
                    if(split.Length == 2)
                        card.Signature = split[1];
                    card.Owner = user.UserId;
                    user.ActiveCards[0] = card;
                }

                await MessageHandler.SendMessage(idList, $"Registered as {user.ActiveCards[0].Name}.");
            }
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

            string[] split = str.Split(' ');

            if(split.Length > 2)
            {
                await MessageHandler.SendMessage(idList, $"You can't use more than 2 parameters. Example Use: \"addcard vrfamily (Osiris)\"");
            }
            else
            {
                var card = CardRegistration.RegisterCard(str);
                if(split.Length == 2)
                    card.Signature = split[1];
                card.Owner = user.UserId;
                user.ActiveCards.Add(card);

                await MessageHandler.SendMessage(idList, $"Added card {user.ActiveCards[user.ActiveCards.Count-1].Name} to registration.");
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

    }
}