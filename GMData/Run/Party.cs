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

        public Party(Def.Party def)
        {
            this.name = def.key;
        }

        [JsonConstructor]
        private Party()
        {
        }
    }
}