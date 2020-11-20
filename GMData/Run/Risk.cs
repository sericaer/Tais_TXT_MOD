using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    public class RiskMgr
    {
        [DataVisitorProperty("start")]
        public string start
        {
            set
            {
                GMRoot.runner.risks.Add(new Risk(value));
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Risk : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        [JsonProperty]
        public string key;

        [JsonProperty]
        public decimal percent { get; set; }

        public bool isEnd => percent >= 100;

        public string endEvent => def.endEvent;

        private Def.Risk def => GMRoot.define.risks.Single(x => x.key == key);

        public Risk(string key)
        {
            this.key = key;
            this.percent = 0.0M;
        }

        internal void DaysInc()
        {
            percent += 100 / (decimal)def.cost_days;
        }
    }
}