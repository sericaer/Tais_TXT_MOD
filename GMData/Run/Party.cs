using System;
using System.Collections.Generic;
using System.Linq;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Party
    {
        [JsonProperty, DataVisitorProperty("type")]
        public string key;

        public Def.Party def => GMRoot.define.parties.Single(x => x.key == key);

        public Party(Def.Party def)
        {
            this.key = def.key;
        }

        [JsonConstructor]
        private Party()
        {
        }

        internal decimal getRelation(string partyName)
        {
            return (decimal)def.relation.Single(x => x.peer == partyName).value;
        }
    }
}