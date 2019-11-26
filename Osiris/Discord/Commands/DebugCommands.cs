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
    public class DebugCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }

        [Command("datawipe")]
        public async Task DataWipe()
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "User and combat data cleared. Reboot bot to take effect.");

            CombatHandler.ClearCombatData(idList);
            UserHandler.ClearUserData();
        }

        [Command("hug")]
        public async Task Hug([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "I refuse to touch you, mortal.");
        }

        [Command("commands")]
        public async Task Commands()
        {
            ContextIds idList = new ContextIds(Context);
            string str = "";
            str += "[] signifies an optional input, {} signifies required input";
            str += "\n**DEBUG:**\n";
            str += "_ping_: Osiris responds with pong. Used to check if he is responding.\n";
            str += "_datawipe_: Wipes all data. Requires a reboot after use.\n";
            str += "_commands_: Displays all commands, excluding the secret ones.\n";
            str += "\n**BASIC**\n";
            str += "_togglecelestial {player}_: Gives specified user celestial status. Users with celestial status have access to special cards and can start large battles.\n";
            str += "_setcard {card}_: Sets your card to a specified card.\n";
            str += "_addcardnext {card}_: Adds the specified card to your list of cards.\n";
            str += "_removecard {card}_: Removes all cards with the specified name.\n";
            str += "_sigset_ [n] [signature]_: Sets your nth card's signature to the specified signature. If n is blank, it assumes your first card. If signature is blank, your signature will become blank.\n";
            str += "_mycards_: Lists your active cards.\n";
            str += "_skillcheck {user}_: Displays the selected user's card. Alternatively, replace user with a card name and it will give you the specified card's info.\n";
            str += "\n**COMBAT**\n";
            str += "_duel {user}_: Sends a duel request to the user.\n";
            str += "_use {move}_: This command is, ironically, unused for now.\n";
            str += "_jointeam {user}_: Join the specified user's team, if they are in a duel.\n";
            str += "_newteam {user}_: Creates a new team in the specified user's duel.\n";
            str += "_round_: Displays the current round info.\n";
            str += "_forfeit_: Exit combat.";
            await MessageHandler.SendMessage(idList, str);   
        }

    }
}