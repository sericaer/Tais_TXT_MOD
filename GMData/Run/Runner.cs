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

        [JsonProperty, DataVisitorProperty("adjust_economy")]
        public AdjustEconomy adjust_economy;

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

            adjust_economy = new AdjustEconomy(GMRoot.define.economy);

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
            //adjust_economy.incomeAdjusts.ForEach(adjust =>
            //{
            //    adjust.effect_pop_consume.Subscribe(x =>
            //    {
            //        foreach (var consume in pops.SelectNotNull(p => p.consume))
            //        {
            //            consume.SetBuffer(adjust.key, x * consume.baseValue.Value * 0.01);
            //        }
            //    });
            //});

            //adjust_economy.incomeAdjusts.Single(x => x.key == "POP_TAX").percent.Subscribe(p =>
            //{
            //    foreach (var tax in pops.SelectNotNull(pop => pop.tax))
            //    {
            //        tax.SetBuffer("POP_TAX", p * tax.baseValue?.Value * 0.01);
            //    }
            //});

            adjust_economy.outputAdjusts.Single(x=>x.key == "ADMIN").percent.Subscribe(p=>
            {
                foreach (var admin in pops.SelectNotNull(pop => pop.adminExpend))
                {
                    admin.SetBuffer("ADMIN", p * admin.baseValue?.Value * 0.01);
                }
            });

            adjust_economy.outputAdjusts.Single(x => x.key == "CHAOTING").level.obs.Subscribe(chaoting.reportTaxLevel);

            adjust_economy.popTaxLevel.Subscribe(level =>
                          {
                              var def = GMRoot.define.economy.adjust_pop_tax.levels[level - 1];

                              foreach (var tax in pops.SelectNotNull(pop => pop.tax))
                              {
                                  tax.SetBuffer("POP_TAX", def.percent * tax.baseValue?.Value * 0.01);
                              }

                              foreach (var consume in pops.SelectNotNull(pop => pop.consume))
                              {
                                  consume.SetBuffer("POP_TAX", def.effect_pop_consume * consume.baseValue?.Value * 0.01);
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
