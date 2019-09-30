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

    }
}