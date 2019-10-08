using System.Collections.Generic;

namespace Osiris
{
    public class Team
    {
        List<UserAccount> Members { get; }

        public Team()
        {

        }

        public Team(bool newteam)
        {
            Members = new List<UserAccount>();
        }
    }
}