using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public bool CombatEnded { get; set; }
        
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
            CombatEnded = false;
        }

        public void FixTurnNumber()
        {
            foreach(BasicCard card in CardList)
            {
                if(card.IsTurn && CardList.IndexOf(card) != TurnNumber)
                {
                    TurnNumber = CardList.IndexOf(card);
                }
            }
        }

        public Team CreateNewTeam()
        {
            Team newteam = new Team(true);
            Teams.Add(newteam);
            newteam.TeamNum = Teams.Count;
            
            return newteam;
        }

        public async Task AddPlayerToCombat(UserAccount user, Team team)
        {
            Players.Add(user);
            await PassiveUpdatePlayerJoined();

            foreach(BasicCard card in user.ActiveCards)
            {
                CardList.Add(card);
                if(card.HasPassive && card.Passive.UpdateJoinCombat)
                    if(!card.Passive.RequiresAsync)
                        card.Passive.Update(this, card);
                    else
                        await card.Passive.UpdateAsync(this, card);
            }
            
            team.Members.Add(user);
            user.CombatRequest = 0;
            user.CombatID = CombatId;
            user.TeamNum = team.TeamNum;
        }

        public async Task PassiveUpdatePlayerJoined()
        {
            foreach(BasicCard card in CardList)
            {
                if(card.HasPassive)
                {
                    if(card.Passive.UpdatePlayerJoin)
                    {
                        if(!card.Passive.RequiresAsync)
                            card.Passive.Update(this, card);
                        else
                            await card.Passive.UpdateAsync(this, card);
                    }
                }
            }
        }

        public async Task PassiveUpdatePlayerLeft()
        {
            foreach(BasicCard card in CardList)
            {
                if(card.HasPassive)
                {
                    if(card.Passive.UpdatePlayerLeave)
                    {
                        if(!card.Passive.RequiresAsync)
                            card.Passive.Update(this, card);
                        else
                            await card.Passive.UpdateAsync(this, card);
                    }
                }
            }
        }

        public Team GetTeam(UserAccount player)
        {
            return Teams[player.TeamNum-1];
        }

        public Team GetTeam(BasicCard player)
        {
            return GetTeam(UserHandler.GetUser(player.Owner));
        }

        public BasicCard GetCardTurn()
        {
            return CardList[TurnNumber];
        }

        public List<BasicCard> SearchForMarker(int turnNum)
        {
            List<BasicCard> marked = new List<BasicCard>();

            foreach(BasicCard card in CardList)
            {
                if(card.SearchForMarker(turnNum) != null)
                {
                    marked.Add(card);
                }
            }

            return marked;
        }

        //Returns a list of valid targets for a standard AOE move that hits the enemy team
        public List<BasicCard> GetAOEEnemyTargets() 
        {
            List<BasicCard> targets = new List<BasicCard>();

            foreach(BasicCard card in CardList)
            {
                if(!card.Dead && (GetTeam(card).TeamNum != GetTeam(GetCardTurn()).TeamNum || (card.IsPuppet && GetTeam(card).TeamNum == GetTeam(GetCardTurn()).TeamNum)))
                {
                    targets.Add(card);
                }
            }
            return targets;
        }

        //Same as GetAOEEnemyTargets, but includes dead players.
        public List<BasicCard> GetAOEEnemyTargetsID()
        {
            List<BasicCard> targets = new List<BasicCard>();

            foreach(BasicCard card in CardList)
            {
                if(GetTeam(card).TeamNum != GetTeam(GetCardTurn()).TeamNum || (card.IsPuppet && GetTeam(card).TeamNum == GetTeam(GetCardTurn()).TeamNum))
                {
                    targets.Add(card);
                }
            }
            return targets;
        }

        //Returns a list of dead enemies
        public List<BasicCard> GetAOEDeadEnemyTargets() 
        {
            List<BasicCard> targets = new List<BasicCard>();

            foreach(BasicCard card in CardList)
            {
                if(card.Dead && (GetTeam(card).TeamNum != GetTeam(GetCardTurn()).TeamNum || (card.IsPuppet && GetTeam(card).TeamNum == GetTeam(GetCardTurn()).TeamNum)))
                {
                    targets.Add(card);
                }
            }
            return targets;
        }

        //Returns a list of valid targets for a standard AOE move that hits only the ally team
        public List<BasicCard> GetAOEAllyTargets() 
        {
            List<BasicCard> targets = new List<BasicCard>();

            foreach(BasicCard card in CardList)
            {
                if(!card.Dead && GetTeam(card).TeamNum == GetTeam(GetCardTurn()).TeamNum && !card.IsPuppet)
                {
                    targets.Add(card);
                }
            }
            return targets;
        }

        //Same as GetAOEAllyTargets but, includes dead players.
        public List<BasicCard> GetAOEAllyTargetsID() 
        {
            List<BasicCard> targets = new List<BasicCard>();

            foreach(BasicCard card in CardList)
            {
                if(GetTeam(card).TeamNum == GetTeam(GetCardTurn()).TeamNum && !card.IsPuppet)
                {
                    targets.Add(card);
                }
            }
            return targets;
        }

        //Returns a list of dead allies
        public List<BasicCard> GetAOEDeadAllyTargets() 
        {
            List<BasicCard> targets = new List<BasicCard>();

            foreach(BasicCard card in CardList)
            {
                if(card.Dead && GetTeam(card).TeamNum == GetTeam(GetCardTurn()).TeamNum && !card.IsPuppet)
                {
                    targets.Add(card);
                }
            }
            return targets;
        }

    }
}