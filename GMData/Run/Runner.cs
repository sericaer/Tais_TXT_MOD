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
            InterfaceAssociate();

            date = new Date(init.start_date);

            taishou = new Taishou(init.taishou);

            parties = Party.Init(GMRoot.define.parties);

            departs = Depart.Init(GMRoot.define.departs, out pops);

            chaoting = new Chaoting(GMRoot.define.chaoting, pops.Where(x=>x.def.is_collect_tax).Sum(x=>x.num.Value));

            economy = new Economy(GMRoot.define.economy);
        }

        [JsonConstructor]
        private Runner()
        {
            Visitor.SetVisitData(this);
        }

        private void InterfaceAssociate()
        {
            Taishou.FuncGetParty = (name) => parties.Find(x => x.name == name);

            Pop.funcGetDef = (name) => GMRoot.define.pops.Single(x => x.key == name);
            Pop.funcGetDepart = (departName) => departs.Single(x => x.name == departName);
            Pop.funcGetTaxpercent = ()=> economy.incomes.popTax.percent.obs;

            Depart.funcGetPop = (name) => pops.Where(x => x.depart_name == name);
            Depart.funcGetDef = (name) => GMRoot.define.departs.Single(x => x.key == name);

            Chaoting.funcGetParty = (name) => parties.Single(x => x.name == name);
        }

        private void DataAssociate()
        {
            date.DataAssociate();

            pops.ForEach(x => x.DataAssociate());

            departs.ForEach(x => x.DataAssocate());

            chaoting.DataAssocate();

            economy.incomes.popTax.SetObsCurrValue(Observable.CombineLatest(departs.Select(x => x.tax.obs)).Select(x=>x.Sum()));
            economy.outputs.departAdmin.SetObsCurrValue(Observable.CombineLatest(departs.Select(x => x.adminExpend.obs)).Select(x => x.Sum()));
            economy.outputs.reportChaoting.SetObsCurrValue(chaoting.expectMonthTaxValue.obs);
            economy.outputs.reportChaoting.expend = chaoting.ReportMonthTax;

            economy.DataAssocate();

            registerPopNum = Observable.CombineLatest(pops.Where(x => x.def.is_collect_tax).Select(x => x.num.obs),
                                         (IList<double> taxs) => taxs.Sum(y => (int)y)).ToOBSValue();

        }

        public void DaysInc()
        {
            economy.DaysInc();

            date.Inc();
        }

        public bool isEnd()
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
