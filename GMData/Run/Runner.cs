using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;

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

        [JsonProperty, DataVisitorPropertyArray("pop")]
        public List<Pop> pops;

        [JsonProperty, DataVisitorProperty("chaoting")]
        public Chaoting chaoting;

        [JsonProperty]
        public List<Party> partys;

        [JsonProperty]
        public List<Risk> risks;

        public ObservableValue<int> registerPopNum;

        public Runner()
        {
            GMRoot.runner = this;

            date = new Date(GMRoot.initData.start_date);

            taishou = new Taishou(GMRoot.initData.taishou);

            partys = Party.Init(GMRoot.define.parties);

            pops = new List<Pop>();

            departs = Depart.Init(GMRoot.define.departs);

            chaoting = new Chaoting(GMRoot.define.chaoting);

            economy = new Economy(GMRoot.define.economy);

            registerPopNum = Observable.CombineLatest(pops.Where(x=>x.def.is_collect_tax).Select(x=>x.num.obs),
                                                     (IList<double> taxs) => taxs.Sum(y=>(int)y)).ToOBSValue();

            Visitor.SetVisitData(this);
        }

        public static Runner Deserialize(string content)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
