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
        [RequireCelestialAttribute]
        [Command("celestialcommands")]
        public async Task CelestialCmds()
        {
            ContextIds idList = new ContextIds(Context);
            string str = "";
            str += "[] signifies an optional input, {} signifies required input";
            str += "\n**CELESTIAL:**\n";
            str += "_initiateraid {channel}_: Starts the initial raid, allowing people to join. Combat won't be started yet.\n";
            str += "_startraid_: If you are currently in a raid that has not started yet, it will begin.";
            str += "_celcardlist_: Lists all celestial cards.\n";
            str += "_setcard {user} {card}_: Sets the card of another user.\n";
            str += "_addcardnext {card}_: Adds the specified card to your list of cards, allowing you to have multiple cards.\n";
            str += "_sigset_ {user} [n] [signature]_: Sets your nth card's signature to the specified signature. If n is blank, it assumes your first card. If signature is blank, your signature will become blank.\n";
            str += "_echo {channel} {message}_: Osiris says the specified message in the specified channel\n";
            str += "_blind {channel}_: Spams blinding light 10 times in selected channel.\n";
            str += "_forceskip {user}_: Force-skips mentioned user's turn.";
            str += "_forceheal {user}_: Force-heals mentioned user.";
            await MessageHandler.SendMessage(idList, str);
        }

        //Echos your message in the specified channel
        [RequireCelestialAttribute]
        [Command("initiateraid")]
        public async Task InitiateRaid(SocketGuildChannel channel)
        {
            ContextIds idList = new ContextIds(Context);
            idList.ChannelId = channel.Id;
            var user = UserHandler.GetUser(idList.UserId);

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

            //Start raid
            CombatInstance combat = new CombatInstance(idList);

            combat.IsDuel = false;
            var combatId = CombatHandler.NumberOfInstances();
            combat.CombatId = combatId;

            user.CombatRequest = 0;
            user.CombatID = combatId;
            await combat.AddPlayerToCombat(user, combat.CreateNewTeam());

            //await MessageHandler.SendMessage(idList, $"{user.ActiveCards[0].Name} is attacking #{channel.Name}! Combat will begin in 10 minutes.\n```\nUse 0.joincombat to join\n```");
            await MessageHandler.SendMessage(idList, $"{user.ActiveCards[0].Name} challenges the party! Combat will begin in 3 minutes.\n```\nUse 0.joincombat to join\n```");

            CombatHandler.StoreInstance(combatId, combat);
        }

        //Echos your message in the specified channel
        [RequireCelestialAttribute]
        [Command("startraid")]
        public async Task StartRaid()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            var combat = CombatHandler.GetInstance(user.CombatID);
            
            if(!combat.IsDuel && combat.RoundNumber == 0)
            {
                await CombatHandler.InitiateRaid(combat);
            }
        }

        [RequireCelestialAttribute]
        [Command("celcardlist")]
        public async Task CelestialCardList()
        {
            ContextIds idList = new ContextIds(Context);

            string str = "";
            str += "```\n";
            str += $"{new SugarGhubbyCard().Name}\n";
            str += $"{new OdysseusKingCard().Name}\n";
            str += $"{new OdysseusExileCard().Name}\n";
            str += $"{new OdysseusOldCard().Name}\n";
            str += $"{new AchillesCard().Name}\n";
            str += $"{new OTrojanSoldierCard().Name}\n";
            str += $"{new OHectorCard().Name}\n";
            str += $"{new PriamCard().Name}\n";
            str += $"{new PolyphemusCard().Name}\n";
            str += $"{new PolyphemusBlindCard().Name}\n";
            str += $"{new CirceCard().Name}\n";
            str += $"{new OPigCard().Name}\n";
            str += "```\n";

            await MessageHandler.SendMessage(idList, str);
        }

        [RequireCelestialAttribute]
        [Command("setcard")]
        public async Task RegisterCard(SocketGuildUser target, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var card = CardRegistration.RegisterCard(str);

            var nick = Context.Guild.GetUser(target.Id).Nickname;

            if(nick == null)
                card.Signature = target.Username;
            else
                card.Signature = nick;

            card.Owner = user.UserId;

            if(user.ActiveCards.Count == 0)
                user.ActiveCards.Add(card);
            else
                user.ActiveCards[0] = card;

            await MessageHandler.SendMessage(idList, $"Registered {card.Signature} as {user.ActiveCards[0].Name}.");
        }

        [RequireCelestialAttribute]
        [Command("addcardnext")]
        public async Task AddCardNext([Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var card = CardRegistration.RegisterCard(str);
            
            var nick = Context.Guild.GetUser(Context.User.Id).Nickname;

            if(nick == null)
                card.Signature = Context.User.Username;
            else
                card.Signature = nick;

            card.Owner = user.UserId;
            user.ActiveCards.Add(card);

            await MessageHandler.SendMessage(idList, $"Added card {user.ActiveCards[user.ActiveCards.Count-1].Name} to registration.");
        }

        [RequireCelestialAttribute]
        [Command("addcardnext")]
        public async Task AddCardNext(SocketGuildUser target, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);
            idList.UserId = user.UserId;

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.UserInCombat(idList);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var card = CardRegistration.RegisterCard(str);
            
            var nick = Context.Guild.GetUser(user.UserId).Nickname;

            if(nick == null)
                card.Signature = Context.Guild.GetUser(user.UserId).Username;
            else
                card.Signature = nick;

            card.Owner = user.UserId;
            user.ActiveCards.Add(card);

            await MessageHandler.SendMessage(idList, $"Added card {user.ActiveCards[user.ActiveCards.Count-1].Name} to registration.");
        }

        [RequireCelestialAttribute]
        [Command("sigset")]
        public async Task SigSet(SocketGuildUser target, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

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

            await MessageHandler.SendMessage(idList, $"Changed signature from {user.ActiveCards[0].Signature} to {str}.");

            user.ActiveCards[0].Signature = str;
        }

        [RequireCelestialAttribute]
        [Command("sigset")]
        public async Task SigSet(SocketGuildUser target, int i, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

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

            if(i > 0 && i <= user.ActiveCards.Count)
            {
                await MessageHandler.SendMessage(idList, $"Changed signature from {user.ActiveCards[i-1].Signature} to {str}.");

                user.ActiveCards[i-1].Signature = str;
            }
            else
            {
                await MessageHandler.SendMessage(idList, $"The number you choose must be no less than one and no greater than the number of cards you own.");
            }
        }

        [RequireCelestialAttribute]
        [Command("sigset")]
        public async Task SigSet(SocketGuildUser target, int i)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(target.Id);

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

            if(i > 0 && i <= user.ActiveCards.Count)
            {
                await MessageHandler.SendMessage(idList, $"Changed signature from {user.ActiveCards[i-1].Signature} to a blank signature.");

                user.ActiveCards[i-1].Signature = "";
            }
            else
            {
                await MessageHandler.SendMessage(idList, $"The number you choose must be no less than one and no greater than the number of cards you own.");
            }
        }

        //Echos your message in the specified channel
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("echo")]
        public async Task Echo(SocketGuildChannel channel, [Remainder] string str)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            idList.ChannelId = channel.Id;
            
            await MessageHandler.SendMessage(idList, $"{str}");
        }

        //Send lots of blinding messages
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("blind")]
        public async Task Blind(SocketGuildChannel channel)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            idList.ChannelId = channel.Id;

            for(int i = 0; i < 10; i++)
            {
                await MessageHandler.SendEmbedMessage(idList, "", OsirisEmbedBuilder.Blinder());
            }
        }

        //Force skip a player's turn.
        [RequireCelestialAttribute]
        [Command("forceskip")]
        public async Task ForceSkip(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            var targetUser = UserHandler.GetUser(target.Id);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.OtherUserHasNoCards(idList, user, targetUser);
                await UserHandler.OtherUserNotInCombat(idList, targetUser);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var combat = CombatHandler.GetInstance(targetUser.CombatID);

            foreach(BasicCard card in targetUser.ActiveCards)
            {
                if(card.IsTurn)
                {
                    await CombatHandler.SkipTurn(combat, card);
                    await MessageHandler.SendMessage(combat.Location, $"{card.Signature}'s turn was force-skipped by a Celestial!");
                }
            }
        }

        //Disable a user (permanent turnskip debuff)
        [RequireCelestialAttribute]
        [Command("forcedisable")]
        public async Task ForceDisable(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            await ReplyAsync("Shut up i havent implemented this yet");
        }

        //Disable a user (permanent turnskip debuff)
        [RequireCelestialAttribute]
        [Command("forceheal")]
        public async Task ForceHeal(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);
            var targetUser = UserHandler.GetUser(target.Id);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.OtherUserHasNoCards(idList, user, targetUser);
            }
            catch(InvalidUserStateException)
            {
                return;
            }

            var combat = CombatHandler.GetInstance(targetUser.CombatID);

            foreach(BasicCard card in targetUser.ActiveCards)
            {
                if(card.IsTurn)
                {
                    card.CurrentHP = card.TotalHP;
                    card.Dead = false;
                }
            }
        }
    }
}