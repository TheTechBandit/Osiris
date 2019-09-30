using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Osiris.Storage.Implementations;

namespace Osiris.Discord
{
    public class CombatCommands : ModuleBase<SocketCommandContext>
    {
        [Command("duel")]
        public async Task Duel(SocketGuildUser target)
        {
            var fromUser = UserHandler.GetUser(Context.User.Id);
            var toUser = UserHandler.GetUser(target.Id);

            ContextIds idList = new ContextIds(Context);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
                await UserHandler.OtherUserInCombat(idList, toUser);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            //Check that the user did not target themself with the command
            if(fromUser.UserId != toUser.UserId)
            {
                //Set the current user's combat request ID to the user specified
                fromUser.CombatRequest = toUser.UserId;

                //Check if the specified user has a combat request ID that is the current user's ID
                if(toUser.CombatRequest == fromUser.UserId)
                {
                    //Start duel
                    CombatInstance combat = new CombatInstance(idList);

                    combat.IsDuel = true;

                    var combatId = CombatStorage.NumberOfInstances();

                    CombatStorage.StoreInstance(combatId, combat);

                    await Context.Channel.SendMessageAsync($"The duel between {target.Mention} and {Context.User.Mention} will now begin!");
                    combat.Players.Add(fromUser);
                    fromUser.CombatRequest = 0;
                    fromUser.CombatID = combatId;
                    fromUser.TeamNum = 0;

                    combat.Players.Add(toUser);
                    toUser.CombatRequest = 0;
                    toUser.CombatID = combatId;
                    toUser.TeamNum = 1;
                }
                else
                {
                    //Challenge the specified user
                    await Context.Channel.SendMessageAsync($"{target.Mention}, you have been challenged to a duel by {Context.User.Mention}\nUse the \"0.duel [mention target]\" command to accept.");
                }
            }
            else
            {
                //Tell the current user they have are a dum dum
                await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot duel yourself.");
            }
        }

        [Command("forfeit")]
        public async Task ExitCombat()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(Context.User.Id);
            var inst = CombatStorage.RestoreInstance(user.CombatID);
            
            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserNotInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            await CombatHandler.RemovePlayerFromCombat(inst, user);
        }
    }
}