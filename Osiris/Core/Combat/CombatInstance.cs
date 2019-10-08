using System.Collections;
using System.Collections.Generic;
using Osiris.Discord;

namespace Osiris
{
    public class CombatInstance
    {
        public ContextIds Location { get; set; }
        public List<Team> Teams { get; set; }
        public List<UserAccount> Players { get; set; }
        public int TurnNumber { get; set; }
        public bool IsDuel { get; set; }
        
        public CombatInstance()
        {
            
        }

        public CombatInstance(ContextIds loc)
        {
            Location = loc;
            Teams = new List<Team>();
            Players = new List<UserAccount>();
            TurnNumber = 0;
            IsDuel = true;
        }

    }
}