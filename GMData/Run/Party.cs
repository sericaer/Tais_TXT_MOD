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

        public Party(string name)
        {
            this.name = name;
        }

        internal static List<Party> Init(IEnumerable<GMData.Def.Party> partyDefs)
        {
            List<Party> rslt = new List<Party>();

            foreach(var def in partyDefs)
            {
                rslt.Add(new Party(def.key));
            }

            return rslt;
        }

        [JsonConstructor]
        private Party()
        {
        }
    }
}