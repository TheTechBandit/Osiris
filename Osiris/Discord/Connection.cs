using Osiris.Discord.Entities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Osiris.Discord
{
    public class Connection
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly DiscordLogger _logger;

        public Connection(DiscordLogger logger, DiscordSocketClient client, CommandService commands)
        {
            _logger = logger;
            _commands = commands;
            _client = client;
        }

        public async Task ConnectAsync(OsirisBotConfig config)
        {
            _client.Log += _logger.Log;

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();

            _client.JoinedGuild += HandleGuildJoin;

            _commands.CommandExecuted += CommandExecutedAsync;
            
            _client.MessageReceived += MessageRecieved;

            _client.GuildAvailable += HandleConnected;

            _client.UserJoined += HandleUserJoin;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            await Task.Delay(-1);
        }

        #pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        private async Task HandleConnected(SocketGuild guild)
        {
            //Upon connecting, update every user's info in every guild.
            foreach(SocketUser user in guild.Users)
            {
                if(!user.IsBot)
                    UserHandler.UpdateUserInfo(user.Id, user.GetOrCreateDMChannelAsync().Result.Id, user.Username, user.Mention, user.GetAvatarUrl());
            }
        }

        #pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        private async Task HandleUserJoin(SocketGuildUser user)
        {
            UserHandler.UpdateUserInfo(user.Id, user.GetOrCreateDMChannelAsync().Result.Id, user.Username, user.Mention, user.GetAvatarUrl());
        }

        private async Task HandleGuildJoin(SocketGuild guild)
        {
            //Upon joining a guild, create update every user's info and create useraccounts for them
            foreach(SocketUser user in guild.Users)
            {
                UserHandler.UpdateUserInfo(user.Id, user.GetOrCreateDMChannelAsync().Result.Id, user.Username, user.Mention, user.GetAvatarUrl());
            }
        }

        private async Task MessageRecieved(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            //**MOVE CHECK LOGIC**\\
            foreach(BasicCard card in UserHandler.GetUser(message.Author.Id).ActiveCards)
            {
                if(card.IsTurn && UserHandler.GetUser(message.Author.Id).CombatID != -1)
                {
                    var author = UserHandler.GetUser(message.Author.Id);
                    var inst = CombatHandler.GetInstance(author.CombatID);

                    var moveNum = -1;
                    //Loop through the card's moves
                    foreach(BasicMove move in card.Moves)
                    {
                        moveNum++;

                        //If the message contains any of the moves' names
                        if(message.Content.Contains($"{move.Name}"))
                        {
                            //Fail if the user is silenced and they did not use their Basic
                            if(moveNum != 0 && card.IsSilenced())
                            {
                                await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! You are silenced. You may only use your Basic (first move listed on your card).");
                                return;
                            }

                            //Count the inputs, if necessary
                            if(move.Targets >= 1)
                            {
                                //Setup a list of targeted cards
                                List<BasicCard> targets = new List<BasicCard>();

                                //Loop through every mentioned user
                                foreach(SocketUser user in message.MentionedUsers)
                                {
                                    //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
                                    try
                                    {
                                        await UserHandler.OtherUserHasNoCards(CombatHandler.GetInstance(author.CombatID).Location, author, UserHandler.GetUser(user.Id));
                                    }
                                    catch(InvalidUserStateException)
                                    {
                                        return;
                                    }

                                    //If the player has at least 1 card, and they are in the same combat session as the author, add them to the list of targets.
                                    var player = UserHandler.GetUser(user.Id);

                                    //Check if the target is dead, if they are and this move cannot target dead, cancel.
                                    if(player.ActiveCards[0].Dead)
                                    {
                                        if(!move.CanTargetDead)
                                        {
                                            await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target the dead! {player.ActiveCards[0].Signature} is dead.");
                                            return;
                                        }
                                    }

                                    //Check if the target is an enemy (or a teammate that has ALSO been puppeted) and if this move cannot target enemies. If so, cancel
                                    if(player.TeamNum != inst.GetTeam(card).TeamNum || (player.TeamNum == inst.GetTeam(card).TeamNum && player.ActiveCards[0].IsPuppet && card.IsPuppet))
                                    {
                                        if(!move.CanTargetEnemies)
                                        {
                                            await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target enemies!");
                                            return;
                                        }
                                    }

                                    //Check if the target is an enemy and if this move cannot target enemies. If so, cancel
                                    if(player.TeamNum == inst.GetTeam(card).TeamNum && !(player.ActiveCards[0].IsPuppet && card.IsPuppet))
                                    {
                                        if(!move.CanTargetAllies)
                                        {
                                            await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target allies!");
                                            return;
                                        }
                                    }

                                    //Check if the target is untargetable and is not on the same team as the caster. If so, cancel.
                                    if(player.ActiveCards[0].IsUntargetable() && player.TeamNum != author.TeamNum)
                                    {
                                        await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {player.ActiveCards[0].Signature} is untargetable.");
                                        return;
                                    }

                                    //Check if the target is the player using the move. If so and this move cannot target self, cancel.
                                    if(player.ActiveCards[0].Equals(card))
                                    {
                                        if(!move.CanTargetSelf)
                                        {
                                            await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target yourself!");
                                            return;
                                        }
                                    }

                                    if(player.CombatID == author.CombatID)
                                        targets.Add(player.ActiveCards[0]);
                                }

                                //Split the message apart by spaces, and search for 0. followed by any number
                                string[] split = message.Content.Split(' ');
                                foreach(string str in split)
                                {
                                    int parse = 0;
                                    //If the current string contains a 0. and ends with a
                                    if(str.Contains("0.") && str.Length >= 3 && int.TryParse(str.Substring(2), out parse) && parse <= CombatHandler.GetInstance(author.CombatID).CardList.Count && parse >= 1)
                                    {
                                        var target = inst.CardList[parse-1];

                                        if(target.Dead)
                                        {
                                            if(!move.CanTargetDead)
                                            {
                                                await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target the dead! {target.Signature} is dead.");
                                                return;
                                            }
                                        }

                                        //Check if the target is the player using the move. If so and this move cannot target self, cancel.
                                        if(target.Equals(card))
                                        {
                                            if(!move.CanTargetSelf)
                                            {
                                                await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target yourself!");
                                                return;
                                            }
                                        }

                                        //Check if the target is an enemy and if this move cannot target enemies. If so, cancel
                                        if(inst.GetTeam(target).TeamNum != inst.GetTeam(card).TeamNum || (inst.GetTeam(target).TeamNum == inst.GetTeam(card).TeamNum && target.IsPuppet && card.IsPuppet))
                                        {
                                            if(!move.CanTargetEnemies)
                                            {
                                                await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target enemies!");
                                                return;
                                            }
                                        }

                                        //Check if the target is an enemy and if this move cannot target enemies. If so, cancel
                                        if(inst.GetTeam(target).TeamNum == inst.GetTeam(card).TeamNum && !(target.IsPuppet && card.IsPuppet))
                                        {
                                            if(!move.CanTargetAllies)
                                            {
                                                await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {move.Name} cannot target allies!");
                                                return;
                                            }
                                        }

                                        //Check if the target is the player using the move. If so and this move cannot target self, cancel.
                                        if(target.IsUntargetable() && UserHandler.GetUser(target.Owner).TeamNum != author.TeamNum)
                                        {
                                            await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {target.Signature} is untargetable.");
                                            return;
                                        }

                                        targets.Add(target);
                                    }
                                }

                                //Determine if a sole target exists on the enemy team
                                bool soleTargetExists = false;
                                BasicCard soleTarg = new BasicCard(true);
                                foreach(Team team in inst.Teams)
                                {
                                    if(team.TeamNum != author.TeamNum)
                                    {
                                        foreach(UserAccount acc in team.Members)
                                        {
                                            foreach(BasicCard c in acc.ActiveCards)
                                            {
                                                if(c.IsSoleTarget())
                                                {
                                                    soleTargetExists = true;
                                                    soleTarg = c;
                                                }
                                            }
                                        }
                                    }
                                }

                                //If a sole target exists on the enemy team and any target is not a teammate, and that target does not have the "Sole Target" buff, the move fails.
                                if(soleTargetExists)
                                {
                                    foreach(BasicCard target in targets)
                                    {
                                        if(UserHandler.GetUser(target.Owner).TeamNum != author.TeamNum)
                                        {
                                            if(!target.IsSoleTarget())
                                            {
                                                await MessageHandler.SendMessage(inst.Location, $"MOVE FAILED! {soleTarg.Signature} is the sole target of all attacks.");
                                                return;
                                            }
                                        }
                                    }
                                }

                                if(targets.Count <= move.Targets && targets.Count > 0)
                                {
                                    if(!move.OnCooldown)
                                        await CombatHandler.UseMove(inst, card, move, targets);
                                    else
                                        await MessageHandler.SendMessage(inst.Location, "That move is on cooldown!");
                                }
                                return;
                            }
                            else
                            {
                                if(!move.OnCooldown)
                                    await CombatHandler.UseMove(inst, card, move);
                                else
                                    await MessageHandler.SendMessage(inst.Location, "That move is on cooldown!");
                                return;
                            }
                        }
                    }

                    if((message.Content.Contains("Skip") || message.Content.Contains("Pass")) && card.CanPassTurn)
                    {
                        await CombatHandler.SkipTurn(inst, card);
                    }
                    else if((message.Content.Contains("Skip") || message.Content.Contains("Pass")) && !card.CanPassTurn)
                    {
                        await MessageHandler.SendMessage(inst.Location, "You are cursed! You cannot Pass your turn.");
                    }
                }
            }
            //**MOVE CHECK LOGIC END **\\

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasStringPrefix("0.", ref argPos) || 
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;
            
            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            if(UserHandler.GetUser(message.Author.Id).Blocked)
            {
                await context.Channel.SendMessageAsync($"{context.User.Username} you have been blocked from using Osiris. Please speak to an admin if this was not supposed to happen.");
                return;
            }

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(context, argPos, services: null);

            //Update user's info
            UserHandler.UpdateUserInfo(context.User.Id, context.User.GetOrCreateDMChannelAsync().Result.Id, context.User.Username, context.User.Mention, context.User.GetAvatarUrl());
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                System.Console.WriteLine($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }
                

            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                System.Console.WriteLine($"Command [{command.Value.Name}] executed for -> [{context.User.Username}]");
                return;
            }
                

            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
        }

    }
}