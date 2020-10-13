using System;
using System.Collections.Generic;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    public class Party
    {
        [DataVisitorProperty("type")]
        public string name;

        public Party(Def.PartyDef def)
        {
            this.name = def.name;
        }

        internal static List<Party> Init(List<Def.PartyDef> partyDefs)
        {
            List<Party> rslt = new List<Party>();

            foreach(var def in partyDefs)
            {
                rslt.Add(new Party(def));
            }

            return rslt;
        }

        [JsonConstructor]
        private Party()
        {
        }
    }
}