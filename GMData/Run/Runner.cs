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
            economy.incomeAdjusts.ForEach(adjust =>
            {
                adjust.effect_pop_tax?.Subscribe(x =>
                {
                    foreach (var tax in pops.SelectNotNull(p => p.tax))
                    {
                        tax.SetBuffer(adjust.key, tax.baseValue.Value * x);
                    }
                });
            });

            economy.outputAdjusts.ForEach(adjust =>
            {
                adjust.effect_spend_admin?.Subscribe(x =>
                {
                    foreach (var admin in pops.SelectNotNull(p => p.adminExpend))
                    {
                        admin.SetBuffer(adjust.key, admin.baseValue.Value * x);
                    }
                });

                adjust.effect_report_chaoting?.Subscribe(x =>
                {

                });
            });


            pops.SelectNotNull(pop => pop.tax).Select(tax => tax.value)
                .CombineLatest()
                .Subscribe(taxes =>
                {
                    economy.incomeDetails.Single(x => x.type == IncomeDetail.TYPE.POP_TAX)
                           .Value.OnNext(taxes.Sum());
                });

            pops.SelectNotNull(pop => pop.adminExpend).Select(admin => admin.value)
                .CombineLatest()
                .Subscribe(admin =>
                {
                    economy.outputDetails.Single(x => x.type == OutputDetail.TYPE.ADMIN)
                           .Value.OnNext(admin.Sum());
                });
        }
            //private void InterfaceAssociate()
            //{
            //    Taishou.FuncGetParty = (name) => parties.Find(x => x.name == name);

            //    Pop.funcGetDef = (name) => GMRoot.define.pops.Single(x => x.key == name);
            //    Pop.funcGetDepart = (departName) => departs.Single(x => x.name == departName);

            //    Depart.funcGetPop = (name) => pops.Where(x => x.depart_name == name);
            //    Depart.funcGetDef = (name) => GMRoot.define.departs.Single(x => x.key == name);

            //    Chaoting.funcGetParty = (name) => parties.Single(x => x.name == name);
            //}

            //private void DataAssociate()
            //{
            //    date.DataAssociate();

            //    pops.ForEach(pop =>
            //    {
            //        pop.DataAssociate(economy.incomes.popTax.pop_tax_effect.obs, economy.incomes.popTax.pop_consume_effect.obs);
            //    });

            //    departs.ForEach(x => x.DataAssocate());

            //    chaoting.DataAssocate();

            //    economy.incomes.popTax.SetObsCurrValue(Observable.CombineLatest(departs.Select(x => x.tax.obs)).Select(x=>x.Sum()));
            //    economy.outputs.departAdmin.SetObsCurrValue(Observable.CombineLatest(departs.Select(x => x.adminExpend.obs)).Select(x => x.Sum()));
            //    economy.outputs.reportChaoting.SetObsCurrValue(Observable.CombineLatest(chaoting.expectMonthTaxValue.obs, 
            //                                                  economy.outputs.reportChaoting.percent.obs, (x, y)=> x*y/100));
            //    economy.outputs.reportChaoting.expend = chaoting.ReportMonthTax;

            //    economy.DataAssocate();

            //    registerPopNum = Observable.CombineLatest(departs.Select(x=>x.popNum.obs)).Select(x=>x.Sum()).ToOBSValue();
            //}

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
