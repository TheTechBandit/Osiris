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

        private async Task HandleConnected(SocketGuild guild)
        {
            //Upon connecting, update every user's info in every guild.
            foreach(SocketUser user in guild.Users)
            {
                if(!user.IsBot)
                    UserHandler.UpdateUserInfo(user.Id, user.GetOrCreateDMChannelAsync().Result.Id, user.Username, user.Mention, user.GetAvatarUrl());
            }
        }

        private async Task HandleUserJoin(SocketGuildUser user)
        {
            UserHandler.UpdateUserInfo(user.Id, user.GetOrCreateDMChannelAsync().Result.Id, user.Username, user.Mention, user.GetAvatarUrl());
        }

        private async Task HandleGuildJoin(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync("**It begins.**");

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
                if(card.IsTurn)
                {
                    //Loop through the card's moves
                    foreach(BasicMove move in card.Moves)
                    {
                        //If the message contains any of the moves' names
                        if(message.Content.Contains($"{move.Name}"))
                        {
                            var author = UserHandler.GetUser(message.Author.Id);
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
                                        targets.Add(CombatHandler.GetInstance(author.CombatID).CardList[parse-1]);
                                    }
                                }

                                if(targets.Count <= move.Targets)
                                    await CombatHandler.UseMove(CombatHandler.GetInstance(author.CombatID), card, move, targets);
                                return;
                            }
                            else
                            {
                                await CombatHandler.UseMove(CombatHandler.GetInstance(author.CombatID), card, move);
                                return;
                            }
                        }
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