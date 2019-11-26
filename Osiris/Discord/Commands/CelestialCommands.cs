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
    public class CelestialCommands : ModuleBase<SocketCommandContext>
    {
        [RequireCelestialAttribute]
        [Command("celestialcommands")]
        public async Task CelestialCmds()
        {
            ContextIds idList = new ContextIds(Context);
            string str = "";
            str += "[] signifies an optional input, {} signifies required input";
            str += "\n**CELESTIAL:**\n";
            str += "_addcardnext {card}_: Adds the specified card to your list of cards, allowing you to have multiple cards.\n";
            str += "_sigset_ {user} [n] [signature]_: Sets your nth card's signature to the specified signature. If n is blank, it assumes your first card. If signature is blank, your signature will become blank.\n";
            str += "_echo {channel} {message}_: Osiris says the specified message in the specified channel\n";
            str += "_blind {channel}_: Spams blinding light 10 times in selected channel.\n";
            await MessageHandler.SendMessage(idList, str);
        }

        [RequireCelestialAttribute]
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

            if(card.RequiresCelestial && !user.Celestial && !card.Hidden)
            {
                await MessageHandler.SendMessage(idList, "That card requires Celestial rank.");
                return;
            }
            else if(card.Hidden && !user.Celestial)
                card = CardRegistration.RegisterCard("vrfamily");
            
            var nick = Context.Guild.GetUser(Context.User.Id).Nickname;

            if(nick == null)
                card.Signature = Context.User.Username;
            else
                card.Signature = nick;

            card.Owner = user.UserId;
            user.ActiveCards.Add(card);

            await MessageHandler.SendMessage(idList, $"Added card {user.ActiveCards[user.ActiveCards.Count-1].Name} to registration.");
        }

        [RequireCelestialAttribute]
        [Command("sigset")]
        public async Task SigSet(SocketGuildUser target, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

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

        [RequireCelestialAttribute]
        [Command("sigset")]
        public async Task SigSet(SocketGuildUser target, int i, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

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

        [RequireCelestialAttribute]
        [Command("sigset")]
        public async Task SigSet(SocketGuildUser target, int i)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

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

        //Echos your message in the specified channel
        [RequireCelestialAttribute]
        [Command("echo")]
        public async Task Echo(SocketGuildChannel channel, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            idList.ChannelId = channel.Id;
            
            await MessageHandler.SendMessage(idList, $"{str}");
        }

        //Send lots of blinding messages
        [RequireCelestialAttribute]
        [Command("blind")]
        public async Task Blind(SocketGuildChannel channel)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            idList.ChannelId = channel.Id;

            for(int i = 0; i < 10; i++)
            {
                await MessageHandler.SendEmbedMessage(idList, "", OsirisEmbedBuilder.Blinder());
            }
        }

        //Disable a user (permanent turnskip debuff)
        [RequireCelestialAttribute]
        [Command("forcedisable")]
        public async Task ForceDisable(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            await ReplyAsync("Shut up i havent implemented this yet");
        }
    }
}