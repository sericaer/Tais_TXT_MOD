using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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

        [JsonProperty, DataVisitorPropertyArray("pop")]
        public List<Pop> pops;

        [JsonProperty, DataVisitorProperty("chaoting")]
        public Chaoting chaoting;

        [JsonProperty]
        public List<Party> parties;

        [JsonProperty]
        public List<Risk> risks;

        [DataVisitorProperty("risk")]
        public RiskMgr riskMgr;

        public ObservableValue<int> registerPopNum;

        public static Runner Generate()
        {
            var runner = new Runner(GMRoot.initData);
            runner.DataAssociate();

            return runner;
        }

        public static Runner Deserialize(string json)
        {
            var obj = JsonConvert.DeserializeObject<Runner>(json);
            obj.DataAssociate();

            return obj;
        }

        public Runner(Init.InitData init) : this()
        {
            date = new Date(init.start_date);

            taishou = new Taishou(init.taishou);

            parties = Party.Init(GMRoot.define.parties);

            departs = Depart.Init(GMRoot.define.departs, out pops);

            chaoting = new Chaoting(GMRoot.define.chaoting, pops.Where(x=>x.def.is_collect_tax).Sum(x=>x.num.Value));

            economy = new Economy(GMRoot.define.economy);

            risks = new List<Risk>();
            riskMgr = new RiskMgr();
        }

        [JsonConstructor]
        private Runner()
        {
            InterfaceAssociate();

            Visitor.SetVisitData(this);
        }

        private void InterfaceAssociate()
        {
            Taishou.FuncGetParty = (name) => parties.Find(x => x.name == name);

            Pop.funcGetDef = (name) => GMRoot.define.pops.Single(x => x.key == name);
            Pop.funcGetDepart = (departName) => departs.Single(x => x.name == departName);
            
            Depart.funcGetPop = (name) => pops.Where(x => x.depart_name == name);
            Depart.funcGetDef = (name) => GMRoot.define.departs.Single(x => x.key == name);

            Chaoting.funcGetParty = (name) => parties.Single(x => x.name == name);
        }

        private void DataAssociate()
        {
            date.DataAssociate();

            pops.ForEach(pop =>
            {
                pop.DataAssociate(economy.incomes.popTax.pop_tax_effect.obs, economy.incomes.popTax.pop_consume_effect.obs);
            });

            departs.ForEach(x => x.DataAssocate());

            chaoting.DataAssocate();

            economy.incomes.popTax.SetObsCurrValue(Observable.CombineLatest(departs.Select(x => x.tax.obs)).Select(x=>x.Sum()));
            economy.outputs.departAdmin.SetObsCurrValue(Observable.CombineLatest(departs.Select(x => x.adminExpend.obs)).Select(x => x.Sum()));
            economy.outputs.reportChaoting.SetObsCurrValue(Observable.CombineLatest(chaoting.expectMonthTaxValue.obs, 
                                                          economy.outputs.reportChaoting.percent.obs, (x, y)=> x*y/100));
            economy.outputs.reportChaoting.expend = chaoting.ReportMonthTax;

            economy.DataAssocate();

            registerPopNum = Observable.CombineLatest(departs.Select(x=>x.popNum.obs)).Select(x=>x.Sum()).ToOBSValue();
        }

        public void DaysInc()
        {
            economy.DaysInc();

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
