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
                await UserHandler.UserHasNoCards(idList, fromUser);
                await UserHandler.OtherUserHasNoCards(idList, fromUser, toUser);
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
                    var combatId = CombatHandler.NumberOfInstances();
                    combat.CombatId = combatId;

                    fromUser.CombatRequest = 0;
                    fromUser.CombatID = combatId;
                    await combat.AddPlayerToCombat(fromUser, combat.CreateNewTeam());

                    toUser.CombatRequest = 0;
                    toUser.CombatID = combatId;
                    await combat.AddPlayerToCombat(toUser, combat.CreateNewTeam());

                    CombatHandler.StoreInstance(combatId, combat);

                    await CombatHandler.InitiateDuel(combat);
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
                await Context.Channel.SendMessageAsync($"{Context.User.Mention}, stop hitting yourself!");
            }
        }

        [Command("joincombat")]
        public async Task JoinCombat()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(Context.User.Id);

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
            
            var combat = CombatHandler.SearchForRaid(idList);

            if(combat == null)
            {
                await Context.Channel.SendMessageAsync($"_There is no raid here._");
                return;
            }
            else if(combat.Teams.Count == 1)
            {
                user.CombatRequest = 0;
                user.CombatID = combat.CombatId;
                await combat.AddPlayerToCombat(user, combat.CreateNewTeam());
            }
            else
            {
                user.CombatRequest = 0;
                user.CombatID = combat.CombatId;
                await combat.AddPlayerToCombat(user, combat.Teams[1]);
            }

            await Context.Channel.SendMessageAsync($"{user.Mention} joined combat!");
        }

        /*
        [Command("use")]
        public async Task Use([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            str = str.ToLower();

            foreach(BasicCard card in user.ActiveCards)
            {
                foreach(BasicMove move in card.Moves)
                {
                    if(move.Name.ToLower() == str)
                    {
                        //do stuff
                    }
                }
            }
        }
        */

        [Command("jointeam")]
        public async Task JoinTeam(SocketGuildUser target)
        {
            
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(Context.User.Id);
            var targ = UserHandler.GetUser(target.Id);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserHasNoCards(idList, user);
                await UserHandler.UserInCombat(idList);
                await UserHandler.OtherUserNotInCombat(idList, targ);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var combat = CombatHandler.GetInstance(targ.CombatID);

            if(combat.IsDuel)
            {
                await combat.AddPlayerToCombat(user, combat.GetTeam(targ));
                await MessageHandler.SendMessage(idList, $"{user.Mention} has joined {targ.Mention}'s team!");
            }
        }

        [Command("newteam")]
        public async Task NewTeam(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(Context.User.Id);
            var targ = UserHandler.GetUser(target.Id);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserHasNoCards(idList, user);
                await UserHandler.UserInCombat(idList);
                await UserHandler.OtherUserNotInCombat(idList, targ);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var combat = CombatHandler.GetInstance(targ.CombatID);

            if(combat.IsDuel)
            {
                await combat.AddPlayerToCombat(user, combat.CreateNewTeam());
                await MessageHandler.SendMessage(idList, $"{user.Mention} has joined combat and created Team {user.TeamNum}!");
            }
        }

        [Command("forfeit")]
        public async Task ExitCombat()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(Context.User.Id);
            
            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserNotInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var inst = CombatHandler.GetInstance(user.CombatID);

            await CombatHandler.RemovePlayerFromCombat(inst, user);
        }

        [Command("round")]
        public async Task RoundUpdate()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(Context.User.Id);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserNotInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var inst = CombatHandler.GetInstance(user.CombatID);
            var embeds = OsirisEmbedBuilder.RoundStart(inst);
            for(int i = 0; i < embeds.Count; i++)
                await MessageHandler.SendEmbedMessage(inst.Location, "", embeds[i]);
        }

    }
}