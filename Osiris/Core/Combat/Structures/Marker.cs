using System.Collections.Generic;

namespace Osiris
{
    public class Marker
    {
        public string CardOrigin { get; set; }
        public string MoveOrigin { get; set; }
        public int OriginTurnNum { get; set; }
        public bool MarkerBool { get; set; }
        public int MarkerStack { get; set; }
        
        public Marker()
        {

        }
    }
}