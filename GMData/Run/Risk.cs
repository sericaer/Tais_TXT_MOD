using System;
using System.Collections.Generic;
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
    public class Risk
    {
        [JsonProperty]
        public string key;

        [JsonProperty]
        public SubjectValue<double> percent;

        public bool isEnd => percent.Value > 100 || Math.Abs(percent.Value - 100) < 0.0001;

        public string endEvent => def.endEvent;

        private Def.Risk def => GMRoot.define.risks.Single(x => x.key == key);

        public Risk(string key)
        {
            this.key = key;
            this.percent = new SubjectValue<double>(0.0);
        }

        internal void DaysInc()
        {
            percent.Value += 100 / def.cost_days;
        }
    }
}