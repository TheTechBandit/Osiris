using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;

namespace Osiris
{
    public static class CombatHandler
    {
        static CombatHandler()
        {

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
                await CheckTeamElimination(inst, player.TeamNum);
            }
            else
            {
                //Raid user forfeiting stuff here
            }
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
            if(inst.Players.Count <= 1)
            {
                await MessageHandler.UserIsVictor(inst.Location, inst.Players[0]);
            }
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
                }
            }
        }

        public static void EndCombat(CombatInstance inst)
        {
            foreach(UserAccount player in inst.Players)
            {
                player.CombatID = -1;
            }
        }

    }
}