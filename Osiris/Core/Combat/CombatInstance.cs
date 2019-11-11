using System.Collections;
using System.Collections.Generic;
using Osiris.Discord;

namespace Osiris
{
    public class CombatInstance
    {
        public ContextIds Location { get; set; }
        public int CombatId { get; set; }
        public List<Team> Teams { get; set; }
        public List<UserAccount> Players { get; set; }
        public List<BasicCard> CardList { get; set; }
        public int RoundNumber { get; set; }
        public int TurnNumber { get; set; }
        public bool IsDuel { get; set; }
        
        public CombatInstance()
        {
            
        }

        public CombatInstance(ContextIds loc)
        {
            Location = loc;
            CombatId = -1;
            Teams = new List<Team>();
            CardList = new List<BasicCard>();
            Players = new List<UserAccount>();
            RoundNumber = 0;
            TurnNumber = 0;
            IsDuel = true;
        }

        public Team CreateNewTeam()
        {
            Team newteam = new Team(true);
            Teams.Add(newteam);
            newteam.TeamNum = Teams.Count;
            
            return newteam;
        }

        public void AddPlayerToCombat(UserAccount user, Team team)
        {
            Players.Add(user);
            foreach(BasicCard card in user.ActiveCards)
            {
                CardList.Add(card);
            }
            team.Members.Add(user);
            user.CombatRequest = 0;
            user.CombatID = CombatId;
            user.TeamNum = team.TeamNum;
        }

        public Team GetTeam(UserAccount player)
        {
            return Teams[player.TeamNum-1];
        }

        public BasicCard GetCardTurn()
        {
            return CardList[TurnNumber];
        }

    }
}