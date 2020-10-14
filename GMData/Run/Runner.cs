using System;
using System.Collections.Generic;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    public class Runner
    {
        [JsonProperty, DataVisitorProperty("date")]
        public Date date;

        [JsonProperty, DataVisitorProperty("economy")]
        public Economy economy;

        [JsonProperty, DataVisitorProperty("taishou")]
        public Taishou taishou;

        [JsonProperty, DataVisitorPropertyArray("depart")]
        public List<Depart> departs;

        [JsonProperty, DataVisitorPropertyArray("depart")]
        public List<Pop> pops;

        [JsonProperty, DataVisitorProperty("chaoting")]
        public Chaoting chaoting;

        [JsonProperty]
        public List<Party> partys;

        [JsonProperty]
        public List<Risk> risks;

        public Runner()
        {
            taishou = new Taishou(GMRoot.initData.taishou);

            date = new Date(1,1,1);

            economy = new Economy(GMRoot.define.economy);

            Visitor.SetVisitData(this);
        }

        public static Runner Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void DaysInc()
        {
            date.Inc();
        }

        public bool isEnd()
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
