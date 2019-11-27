using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;
using Osiris.Storage.Implementations;

namespace Osiris
{
    public static class CombatHandler
    {
        public static readonly string filepath;
        private static Dictionary<int, CombatInstance> _dic;
        private static JsonStorage _jsonStorage;

        static CombatHandler()
        {
            System.Console.WriteLine("Loading Combat Instances...");
            
            //Access JsonStorage to load user list into memory
            filepath = "Core/Combat/CombatInstances";

            _dic = new Dictionary<int, CombatInstance>();
            _jsonStorage = new JsonStorage();

            foreach(KeyValuePair<int, CombatInstance> entry in _jsonStorage.RestoreObject<Dictionary<int, CombatInstance>>(filepath))
            {
                _dic.Add(entry.Key, (CombatInstance)entry.Value);
            }

            System.Console.WriteLine($"Successfully loaded {_dic.Count} combat instances.");
        }

        public static void SaveInstances()
        {
            System.Console.WriteLine("Saving combat...");
            _jsonStorage.StoreObject(_dic, filepath);
        }

        public static void StoreInstance(int key, CombatInstance inst)
        {
            if (_dic.ContainsKey(key))
            {
                _dic[key] = inst;
                return;
            }

            _dic.Add(key, inst);

            SaveInstances();
        }

        public static CombatInstance GetInstance(int key)
        {
            if(!_dic.ContainsKey(key))
                throw new ArgumentException($"The provided key '{key}' wasn't found.");
            return _dic[key];
        }

        public static CombatInstance SearchForRaid(ContextIds idList)
        {
            CombatInstance inst = null;

            foreach(KeyValuePair<int, CombatInstance> entry in _dic)
            {
                if(!entry.Value.IsDuel && entry.Value.Location.ChannelId == idList.ChannelId)
                    inst = entry.Value;
            }

            return inst;
        }

        public static int NumberOfInstances()
        {
            return _dic.Count;
        }

        public static void ClearCombatData(ContextIds ids)
        {
            System.Console.WriteLine("Deleting all combat instances.");
            Dictionary<ulong, CombatInstance> emptyDic = new Dictionary<ulong, CombatInstance>();
            emptyDic.Add(0, new CombatInstance(ids));

            _jsonStorage.StoreObject(emptyDic, filepath);
        }

        public static async Task RemovePlayerFromCombat(CombatInstance inst, UserAccount player)
        {
            var team = inst.GetTeam(player);

            inst.Players.Remove(player);

            var turnskip = false;
            BasicCard skipCard = null;
            foreach(BasicCard card in player.ActiveCards)
            {
                //Remove any markers corresponding to the cards to be removed from combat
                var turnnum = inst.CardList.IndexOf(card);
                foreach(BasicCard card2 in inst.CardList)
                {
                    for (int i = card2.Markers.Count - 1; i >= 0; i--)  
                    {  
                        if(card2.Markers[i].OriginTurnNum == turnnum)
                            card2.Markers.RemoveAt(i);
                    }
                }

                inst.CardList.Remove(card);

                if(turnnum <= inst.TurnNumber)
                    inst.TurnNumber--;
                if(card.IsTurn)
                {
                    turnskip = true;
                    skipCard = card;
                }
            }

            inst.GetTeam(player).Members.Remove(player);
            player.ResetCombatFields(inst.IsDuel);

            await inst.PassiveUpdatePlayerLeft();

            await MessageHandler.UserForfeitsCombat(inst.Location, player);

            if(inst.IsDuel)
            {
                await CheckTeamElimination(inst, team);
            }

            if(turnskip)
                await SkipTurn(inst, skipCard);

            SaveInstances();
        }

        public static async Task CheckPlayerDeath(CombatInstance inst)
        {
            foreach(BasicCard card in inst.CardList)
            {
                if(!card.Dead && card.CurrentHP <= 0)
                {
                    card.Dead = true;
                    await MessageHandler.SendMessage(inst.Location, card.GetDeathMessage());
                    await CheckTeamElimination(inst, inst.GetTeam(card));
                }
            }
        }

        public static async Task CheckTeamElimination(CombatInstance inst, Team team)
        {
            var teamCount = team.Members.Count;
            var teamDead = 0;
            foreach(UserAccount player in team.Members)
            {
                foreach(BasicCard card in player.ActiveCards)
                {
                    if(card.Dead)
                        teamDead++;
                }
            }

            if(teamCount == teamDead)
            {
                await MessageHandler.TeamEliminated(inst.Location, team.TeamNum);
                inst.Teams.Remove(team);
                await CheckDuelVictory(inst);
            }
        }

        public static async Task CheckDuelVictory(CombatInstance inst)
        {
            if(inst.Teams.Count <= 1)
            {
                await MessageHandler.TeamVictory(inst.Location, inst.Teams[0].ToString(), inst.Teams[0].TeamNum);
                await CombatHandler.EndCombat(inst);
            }
        }

        public static async Task EndCombat(CombatInstance inst)
        {
            _dic.Remove(inst.Players[0].CombatID);
            inst.CombatEnded = true;

            await MessageHandler.SendEmbedMessage(inst.Location, "**Combat End**", OsirisEmbedBuilder.RoundStart(inst));

            foreach(UserAccount player in inst.Players)
            {
                player.ResetCombatFields(inst.IsDuel);
            }

            UserHandler.SaveUsers();
            CombatHandler.SaveInstances();
        }

        public static async Task InitiateDuel(CombatInstance inst)
        {
            await MessageHandler.SendMessage(inst.Location, $"The duel between {inst.Teams[0].Members[0].Mention} (Team 1) and {inst.Teams[1].Members[0].Mention} (Team 2) will now begin!");
            await NextRound(inst);
        }

        public static async Task InitiateRaid(CombatInstance inst)
        {
            await MessageHandler.SendMessage(inst.Location, $"**COMBAT START!**");
            await NextRound(inst);
        }

        public static async Task NextRound(CombatInstance inst)
        {
            Console.WriteLine("1");
            inst.RoundNumber++;
            inst.TurnNumber = 0;

            Console.WriteLine("2");

            if(inst.CombatEnded)
            {
                await MessageHandler.SendEmbedMessage(inst.Location, "**Combat End**", OsirisEmbedBuilder.RoundStart(inst));
                return;
            }

            Console.WriteLine("3");

            await MessageHandler.SendEmbedMessage(inst.Location, "", OsirisEmbedBuilder.RoundStart(inst));

            Console.WriteLine("4");

            foreach(BasicCard card in inst.CardList)
            {
                await card.RoundTick();
            }

            Console.WriteLine("5");
            await PassiveUpdateRoundStart(inst);

            inst.TurnNumber = -1;

            await Task.Delay(1500);
            
            await NextTurn(inst);
        }

        public static async Task PassiveUpdateRoundStart(CombatInstance inst)
        {
            foreach(BasicCard card in inst.CardList)
            {
                if(card.HasPassive && card.Passive.UpdateRoundStart)
                {
                    if(!card.Passive.RequiresAsync)
                        card.Passive.Update(inst, card);
                    else
                        await card.Passive.UpdateAsync(inst, card);
                }
            }
        }

        public static async Task NextTurn(CombatInstance inst)
        {
            await CheckPlayerDeath(inst);

            if(inst.CombatEnded)
            {
                return;
            }
            
            inst.TurnNumber++;
            if(inst.TurnNumber >= inst.CardList.Count)
            {
                await NextRound(inst);
                return;
            }

            var card = inst.CardList[inst.TurnNumber];
            var user = UserHandler.GetUser(card.Owner);

            //Turn start tick. Moved to turn end. May implement here later.
            //await card.TurnTick();

            card.ApplyBonusActions();

            var skip = false;
            foreach(BuffDebuff eff in card.Effects)
            {
                if(eff.TurnSkip)
                    skip = true;
            }
            
            if(!card.Dead && !skip)
            {
                await MessageHandler.SendEmbedMessage(inst.Location, $"{user.Mention}'s Turn!", OsirisEmbedBuilder.PlayerTurnStatus(inst.CardList[inst.TurnNumber], inst.RoundNumber));
                card.IsTurn = true;
            }
            else
            {
                await card.TurnTick();
                card.IsTurn = false;
                await NextTurn(inst);
            }
        }

        public static async Task UseMove(CombatInstance inst, BasicCard owner, BasicMove move)
        {
            await move.MoveEffect(inst);

            if(owner.Actions <= 0)
            {
                await owner.TurnTick();
                owner.Actions = owner.TotalActions;
                owner.IsTurn = false;
                await NextTurn(inst);
            }
            else
            {
                await MessageHandler.SendMessage(inst.Location, $"It is {owner.Signature}'s turn... Again!");
            }
        }

        public static async Task UseMove(CombatInstance inst, BasicCard owner, BasicMove move, List<BasicCard> targets)
        {
            await move.MoveEffect(inst, targets);

            if(owner.Actions <= 0)
            {
                await owner.TurnTick();
                owner.Actions = owner.TotalActions;
                owner.IsTurn = false;
                await NextTurn(inst);
            }
            else
            {
                await MessageHandler.SendMessage(inst.Location, $"It is {owner.Signature}'s turn... Again!");
            }
        }

        public static async Task SkipTurn(CombatInstance inst, BasicCard player)
        {
            player.IsTurn = false;
            await NextTurn(inst);
        }

    }
}