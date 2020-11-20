using System;
using System.Linq;
using System.Collections.Generic;
using DataVisit;
using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Pop : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        [JsonProperty]
        public string name;

        [JsonProperty]
        public decimal num { get; set; }

        [JsonProperty]
        public ObsBufferedValue adminExpend;

        [JsonProperty]
        public ObsBufferedValue tax;

        [JsonProperty]
        public ObsBufferedValue consume;

        [JsonProperty]
        public Family family
        {
            get
            {
                return _family;
            }
            set
            {
                _family = value;

                if(_family != null)
                {
                    _family.pop = this;
                }
            }
        }

        [DataVisitorProperty("depart")]
        internal Depart depart;

        private Family _family;

        internal Def.Pop def => GMRoot.define.pops.Single(x=>x.key == name);

        internal void DaysInc()
        {

        }

        internal Pop(Depart depart, string name, double num, string party)
        {
            this.name = name;

            this.depart = depart;
            this.num = (decimal)num;

            if(def.is_family)
            {
                var person_num = (int)num / 150;
                this.family = new Family(person_num > 3 ? person_num : 3, party);
            }

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
                this.OBSProperty(x=>x.num).Subscribe(x => adminExpend.SetBaseValue(x * 0.0005M));
            }

            if (tax != null)
            {
                this.OBSProperty(x => x.num).Subscribe(x => tax.SetBaseValue(x * 0.01M));
            }

            if (consume != null)
            {
                this.consume.SetBaseValue((decimal)def.consume.Value);
            }
        }
    }
}