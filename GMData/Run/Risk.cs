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

        private Def.Risk def
        {
            get
            {
                return GMRoot.define.risks.Single(x => x.key == key);
            }
        }

        public Risk(string key)
        {
            this.key = key;
            this.percent = new SubjectValue<double>(0.0);
        }

        internal bool isEnd
        {
            get
            {
                return percent.Value > 100 || Math.Abs(percent.Value - 100) < 0.0001;
            }
        }

        //internal static IEnumerable<string> DaysInc()
        //{
        //    all.RemoveAll(risk => risk.isEnd);

        //    foreach(var risk in all)
        //    {
        //        risk.percent.Value += 100 / risk.def.cost_days;

        //        if(risk.isEnd)
        //        {
        //            yield return risk.def.endEvent;
        //        }
        //    }
        //}

        internal static List<Risk> Init()
        {
            return new List<Risk>();
        }
    }
}