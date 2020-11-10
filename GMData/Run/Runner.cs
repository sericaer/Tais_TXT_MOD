using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
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

        [JsonProperty, DataVisitorProperty("chaoting")]
        public Chaoting chaoting;

        [DataVisitorPropertyArray("pop")]
        public List<Pop> pops => departs.SelectMany(x => x.pops).ToList();

        [JsonProperty, DataVisitorPropertyArray("party")]
        public List<Party> parties;

        [JsonProperty]
        public List<Risk> risks;

        [DataVisitorProperty("risk")]
        public RiskMgr riskMgr;

        public ObservableValue<int> registerPopNum;

        [JsonProperty]
        public List<Adjust> adjusts;

        public static Runner Generate()
        {
            var runner = new Runner(GMRoot.initData);
            return runner;
        }

        public static Runner Deserialize(string json)
        {
            var obj = JsonConvert.DeserializeObject<Runner>(json);
            return obj;
        }

        public Runner(Init.InitData init) : this()
        {
            date = new Date(init.start_date);

            taishou = new Taishou(init.taishou);

            parties = GMRoot.define.parties.Select(def => new Party(def)).ToList();

            departs = GMRoot.define.departs.Select(def => new Depart(def)).ToList();

            chaoting = new Chaoting(GMRoot.define.chaoting, pops.Where(x => x.def.is_collect_tax).Sum(x => x.num.Value));

            economy = new Economy(GMRoot.define.economy);

            adjusts = Enum.GetValues(typeof(Adjust.EType)).Cast<Adjust.EType>().Select(x => new Adjust(x)).ToList();

            risks = new List<Risk>();
            riskMgr = new RiskMgr();

            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Runner()
        {
            Visitor.SetVisitData(this);
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            adjusts.ForEach(ad =>
            {
                switch (ad.etype)
                {
                    case Adjust.EType.POP_TAX:
                        foreach (var tax in pops.SelectNotNull(pop => pop.tax))
                        {
                            tax.SetBuffer(ad.etype.ToString(), ad.levelDef.percent * tax.baseValue?.Value * 0.01);
                        }
                        break;
                    case Adjust.EType.ADMIN_SPEND:
                        foreach (var admin in pops.SelectNotNull(pop => pop.adminExpend))
                        {
                            admin.SetBuffer(ad.etype.ToString(), ad.levelDef.percent * admin.baseValue?.Value * 0.01);
                        }
                        break;
                    case Adjust.EType.REPORT_CHAOTING:
                        ad.level.Subscribe(chaoting.reportTaxLevel);
                        break;
                }

                if(ad.levelDef.effect_pop_consume != null)
                {
                    foreach (var consume in pops.SelectNotNull(pop => pop.consume))
                    {
                        consume.SetBuffer("POP_TAX", ad.levelDef.effect_pop_consume * consume.baseValue?.Value * 0.01);
                    }
                }

            });

            pops.CombineLatestSum(pop => pop.tax?.value)
                .Subscribe(economy.detail.popTax);

            pops.CombineLatestSum(pop => pop.adminExpend?.value)
                .Subscribe(economy.detail.adminSpend);

            chaoting.monthTaxReqort.Subscribe(economy.detail.reportChaoting);
            
        }

        public void DaysInc()
        {
            economy.DaysInc(date);

            date.Inc();

            risks.ForEach(x => x.DaysInc());

            risks.RemoveAll(x => x.isEnd);
        }

        public bool isEnd()
        {
            return taishou.isRevoke;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
