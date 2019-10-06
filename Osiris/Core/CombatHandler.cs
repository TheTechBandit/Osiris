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
            filepath = "Core/CombatInstances";

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
            var teamNum = player.TeamNum;

            inst.Players.Remove(player);
            player.CombatID = -1;
            player.TeamNum = -1;

            await MessageHandler.UserForfeitsCombat(inst.Location, player);

            if(inst.IsDuel)
            {
                await CheckTeamElimination(inst, teamNum);
            }
            else
            {
                //Raid user forfeiting stuff here
            }

            SaveInstances();
        }

        public static async Task CheckTeamElimination(CombatInstance inst, int teamId)
        {
            var teamCount = 0;
            var teamDead = 0;
            foreach(UserAccount player in inst.Players)
            {
                if(player.TeamNum == teamId)
                {
                    teamCount++;
                    if(player.Dead)
                    {
                        teamDead++;
                    }
                }
            }

            if(teamCount == teamDead)
            {
                await MessageHandler.TeamEliminated(inst.Location, teamId);
                await CheckDuelVictory(inst);
            }
        }

        public static async Task CheckDuelVictory(CombatInstance inst)
        {
            //If only 1 player still remains in the combat list, that player wins
            if(inst.Players.Count <= 1)
            {
                await MessageHandler.UserIsVictor(inst.Location, inst.Players[0]);
                CombatHandler.EndCombat(inst);
            }
            //Otherwise, loop through the list of players to check that the only living players have the same team ID. If they do, they win.
            else
            {
                var teamNum = -1;
                string victors = "";
                var teamDetected = false;
                var victory = true;

                //Test if all living players have the same Team ID. If they do, victory is achieved
                foreach(UserAccount player in inst.Players)
                {
                    if(teamDetected)
                    {
                        if(teamNum != player.TeamNum && !player.Dead)
                        {
                            victory = false;
                        }
                        else if(teamNum == player.TeamNum)
                        {
                            victors += $" {player.Mention}";
                        }
                    }

                    if(!player.Dead && !teamDetected)
                    {
                        teamNum = player.TeamNum;
                        victors += $" {player.Mention}";
                        teamDetected = true;
                    }
                }

                if(victory)
                {
                    await MessageHandler.TeamVictory(inst.Location, victors, teamNum);
                    CombatHandler.EndCombat(inst);
                }
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

    }
}