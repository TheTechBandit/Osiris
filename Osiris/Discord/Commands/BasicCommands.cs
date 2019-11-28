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
        [Command("commands")]
        public async Task Commands()
        {
            ContextIds idList = new ContextIds(Context);
            string str = "";
            str += "[] signifies an optional input, {} signifies required input";
            str += "\n**DEBUG:**\n";
            str += "_ping_: Osiris responds with pong. Used to check if he is responding.\n";
            str += "\n**BASIC**\n";
            str += "_commands_: Displays all commands, excluding the secret ones.\n";
            str += "_cardlist_: Lists all cards.\n";
            str += "_setcard {card}_: Sets your card to a specified card. If you misspell, it will give you VRFamily by default.\n";
            str += "_removecard {card}_: Unequips card with the specified name.\n";
            str += "_mycards_: Lists your active cards.\n";
            str += "_check {user}_: Displays the selected user's card. Alternatively, replace user with a card name and it will give you the specified card's info. If used during combat, it will display active effects and cooldowns.\n";
            str += "_info {card name}_: Displays the info for the specified card. If you misspell, it will display VRFamily by default.\n";
            str += "\n**COMBAT**\n";
            str += "_joincombat_: Joins a raid, if there is one.\n";
            str += "_duel {user}_: Sends a duel request to the user.\n";
            str += "_jointeam {user}_: Join the specified user's team, if they are in a duel. Does not work on Raids.\n";
            str += "_newteam {user}_: Creates a new team in the specified user's duel. Does not work on Raids.\n";
            str += "_round_: Displays the current round info.\n";
            str += "_forfeit_: Exit combat. Counts as a loss. If used in a Raid, you are killed.";
            str += "_use {move}_: This command is, ironically, unused for now.\n";
            await MessageHandler.SendMessage(idList, str);   
        }

        [Command("cardlist")]
        public async Task CardList()
        {
            ContextIds idList = new ContextIds(Context);

            string str = "";
            //str += $"{new VRFamilyCard().Name}\n";
            //str += $"{new TouchedCard().Name}\n";
            //str += $"{new GhubCard().Name}\n";
            str += "Use the 0.info {card} command to learn more about each card.\n";
            str += "```\n";
            str += $"{new CuteBunnyCard().Name}\n";
            str += $"{new SpeedyHareCard().Name}\n";
            str += $"{new FluffyAngoraCard().Name}\n";
            str += $"{new AngryJackalopeCard().Name}\n";
            str += "```\n";

            await MessageHandler.SendMessage(idList, str);  
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

            if(card.RequiresCelestial && !user.Celestial && !card.Hidden)
            {
                await MessageHandler.SendMessage(idList, "That card requires Celestial rank.");
                return;
            }
            else if(card.Hidden && !user.Celestial)
                card = CardRegistration.RegisterCard("default");

            if(card.Disabled)
            {
                await MessageHandler.SendMessage(idList, "That card has been temporarily disabled.");
                return;
            }

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

        /*
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
        */

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

        [Command("check")]
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

        [Command("info")]
        public async Task CheckCardInfo([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            var card = CardRegistration.RegisterCard(str);

            if(card.Hidden && !user.Celestial)
                card = CardRegistration.RegisterCard("vrfamily");

            await MessageHandler.SendEmbedMessage(idList, "", OsirisEmbedBuilder.CardList(card));
        }

    }
}