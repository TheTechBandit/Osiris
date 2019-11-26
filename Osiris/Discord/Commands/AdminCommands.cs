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
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("admincommands")]
        public async Task CelestialCmds()
        {
            ContextIds idList = new ContextIds(Context);
            string str = "";
            str += "[] signifies an optional input, {} signifies required input";
            str += "\n**ADMIN:**\n";
            str += "_togglecelestial {player}_: Gives specified user celestial status. Users with celestial status have access to special cards and can start large battles.\n";
            str += "_datawipe_: Wipes all bot data. Dangerous once bot is launched officially. Requires reboot after use.\n";
            str += "_blockuser {user}_: Blocks the specified user from using Osiris commands entirely. Use again to unblock.\n";
            await MessageHandler.SendMessage(idList, str);
        }

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

        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("datawipe")]
        public async Task DataWipe()
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "User and combat data cleared. Reboot bot to take effect.");

            CombatHandler.ClearCombatData(idList);
            UserHandler.ClearUserData();
        }

        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("blockuser")]
        public async Task BlockUser(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

            if(user.Blocked == true)
            {
                user.Celestial = false;
                await MessageHandler.SendMessage(idList, $"{user.Name} is no longer **Command Blocked**!");
            }
            else
            {
                user.Blocked = true;
                await MessageHandler.SendMessage(idList, $"{user.Name} is now **Command Blocked**!");
            }
        }
    }
}