using System;
using System.Linq;
using System.Collections.Generic;
using DataVisit;
using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Pop
    {
        [JsonProperty]
        public string name;

        [JsonProperty]
        public string depart_name;

        [JsonProperty]
        public SubjectValue<double> num;

        [JsonProperty]
        public ObservableBufferedValue adminExpend;

        [JsonProperty]
        public ObservableBufferedValue tax;

        [JsonProperty]
        public ObservableBufferedValue consume;

        [DataVisitorProperty("depart")]
        public Depart depart
        {
            get
            {
                return GMRoot.runner.departs.Single(x => x.name == depart_name);
            }
        }

        internal Def.PopDef def
        {
            get
            {
                return GMRoot.define.pops[name];
            }
        }

        internal static List<Pop> Init(IEnumerable<Depart> departs)
        {
            var all = new List<Pop>();
            foreach(var depart in departs)
            {
                all.AddRange(InitDepartPop(depart));
            }
            return all;
        }

        private static List<Pop> InitDepartPop(Depart depart)
        {
            var pops = new List<Pop>();

            foreach (var pop_init in depart.def.pop_init)
            {
                pops.Add(new Pop(depart.name, pop_init.name, pop_init.num));
            }

            return pops;
        }

        internal void DaysInc()
        {

        }

        internal Pop(string depart_name, string name, int num)
        {
            this.name = name;
            this.depart_name = depart_name;

            this.num = new SubjectValue<double>(num);

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Pop()
        {
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            this.tax = new ObservableBufferedValue(this.num.obs.Select(x => def.is_collect_tax ? x * 0.01 : 0));

            this.adminExpend = new ObservableBufferedValue(this.num.obs.Select(x => def.is_collect_tax ? x * 0.0005 : 0));

            if (def.consume != null)
            {
                this.consume = new ObservableBufferedValue(new SubjectValue<double>(def.consume.Value).obs);
            }
        }
    }
}