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

        [Command("touchmeinthepeepee")]
        public async Task TouchMe()
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "You disgust me, mortal.");
        }

        [Command("hug")]
        public async Task Hug([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "I refuse to touch you, mortal.");
        }

        [Command("slap")]
        public async Task Slap(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            await MessageHandler.SendMessage(idList, $"{user.Mention} slapped {target.Mention} for 50 damage!");
            var targetUser = UserHandler.GetUser(target.Id);
            targetUser.ActiveCards[0].CurrentHP -= 50;
        }

    }
}