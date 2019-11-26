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

        [Command("hug")]
        public async Task Hug([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "I refuse to touch you, mortal.");
        }
        
    }
}