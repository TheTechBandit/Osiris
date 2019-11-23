using System.Collections.Generic;

namespace Osiris
{
    public class Marker
    {
        public string MarkerName { get; set; }
        public string CardOrigin { get; set; }
        public string MoveOrigin { get; set; }
        public int OriginTurnNum { get; set; }
        public bool IgnoreBool { get; set; }
        public bool MarkerBool { get; set; }
        public int MarkerStack { get; set; }

        public Marker()
        {

        }

        public Marker(bool def)
        {
            IgnoreBool = true;
        }

        public new string ToString()
        {
            string extra = "";
            if(MarkerStack != 0)
            {
                extra += $"{MarkerStack} stacks. ";
            }
            if(!IgnoreBool)
            {
                extra += $"{MarkerBool}.";
            }

            return $"**{MarkerName} ({CardOrigin})**- {extra}";
        }
    }
}