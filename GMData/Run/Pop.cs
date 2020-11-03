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
        public SubjectValue<double> num;

        [JsonProperty]
        public ObsBufferedValue adminExpend;

        [JsonProperty]
        public ObsBufferedValue tax;

        [JsonProperty]
        public ObsBufferedValue consume;

        [DataVisitorProperty("depart")]
        internal Depart depart;

        internal Def.Pop def => GMRoot.define.pops.Single(x=>x.key == name);

        internal void DaysInc()
        {

        }

        internal Pop(Depart depart, string name, double num)
        {
            this.name = name;

            this.depart = depart;
            this.num = new SubjectValue<double>(num);

            if(def.is_collect_tax)
            {
                this.tax = new ObsBufferedValue();
                this.adminExpend = new ObsBufferedValue();
            }

            if (def.consume != null)
            {
                this.consume = new ObsBufferedValue();
            }

            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Pop()
        {
            
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            if (adminExpend != null)
            {
                num.Subscribe(x => adminExpend.SetBaseValue(x * 0.0005));
            }

            if (tax != null)
            {
                num.Subscribe(x => tax.SetBaseValue(x * 0.01));
            }

            if (consume != null)
            {
                this.consume.SetBaseValue(def.consume.Value);
            }
        }
    }
}