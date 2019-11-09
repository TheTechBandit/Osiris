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
            foreach(BasicCard card in player.ActiveCards)
            {
                inst.CardList.Remove(card);
            }
            inst.GetTeam(player).Members.Remove(player);
            player.CombatID = -1;
            player.TeamNum = -1;

            await MessageHandler.UserForfeitsCombat(inst.Location, player);

            if(inst.IsDuel)
            {
                await CheckTeamElimination(inst, team);
            }
            else
            {
                //Raid user forfeiting stuff here
            }

            SaveInstances();
        }

        public static async Task CheckTeamElimination(CombatInstance inst, Team team)
        {
            var teamCount = team.Members.Count;
            var teamDead = 0;
            foreach(UserAccount player in team.Members)
            {
                if(player.Dead)
                    teamDead++;
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
                CombatHandler.EndCombat(inst);
            }
        }

        public static void EndCombat(CombatInstance inst)
        {
            _dic.Remove(inst.Players[0].CombatID);

            foreach(UserAccount player in inst.Players)
            {
                player.CombatID = -1;
                player.TeamNum = -1;
            }

            UserHandler.SaveUsers();
            CombatHandler.SaveInstances();
        }

        public static async Task InitiateDuel(CombatInstance inst)
        {
            await MessageHandler.SendMessage(inst.Location, $"The duel between {inst.Teams[0].Members[0].Mention} (Team 1) and {inst.Teams[1].Members[0].Mention} (Team 2) will now begin!");
            await NextRound(inst);
        }

        public static async Task NextRound(CombatInstance inst)
        {
            inst.RoundNumber ++;
            inst.TurnNumber = 0;

            await MessageHandler.SendEmbedMessage(inst.Location, "", OsirisEmbedBuilder.RoundStart(inst));

            inst.TurnNumber = -1;

            await Task.Delay(1500);
            
            await NextTurn(inst);
        }

        public static async Task NextTurn(CombatInstance inst)
        {
            inst.TurnNumber++;
            if(inst.TurnNumber >= inst.CardList.Count)
            {
                await NextRound(inst);
                return;
            }
            var card = inst.CardList[inst.TurnNumber];
            var user = UserHandler.GetUser(card.Owner);
            await MessageHandler.SendEmbedMessage(inst.Location, $"{user.Mention}'s Turn!", OsirisEmbedBuilder.PlayerTurnStatus(inst.CardList[inst.TurnNumber], inst.RoundNumber));
            card.IsTurn = true;
        }

        public static async Task UseMove(CombatInstance inst, BasicCard owner, BasicMove move)
        {

        }

        public static async Task UseMove(CombatInstance inst, BasicCard owner, BasicMove move, List<BasicCard> targets)
        {
            await move.MoveEffect(inst, targets);
            owner.IsTurn = false;
            await NextTurn(inst);
        }

    }
}