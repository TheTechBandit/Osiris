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
        //Disable a user (permanent turnskip debuff)
        [Command("forcedisable")]
        public async Task ForceDisable(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            await ReplyAsync("Shut up i havent implemented this yet");
        }
    }
}