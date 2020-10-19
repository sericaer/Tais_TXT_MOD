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
        internal static Func<string, Depart> funcGetDepart;
        internal static Func<string, Def.Pop> funcGetDef;
        internal static Func<IObservable<double>> funcGetTaxpercent;

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
        public Depart depart => funcGetDepart(depart_name);

        internal Def.Pop def => funcGetDef(name);

        internal void DaysInc()
        {

        }

        internal Pop(string depart_name, string name, double num)
        {
            this.name = name;
            this.depart_name = depart_name;

            this.num = new SubjectValue<double>(num);

            this.tax = new ObservableBufferedValue();
            this.adminExpend = new ObservableBufferedValue();

            if (def.consume != null)
            {
                this.consume = new ObservableBufferedValue();
            }
        }

        internal void DataAssociate()
        {
            var taxBase = this.num.obs.Select(x => def.is_collect_tax ? x * 0.01 : 0);

            this.tax.SetBaseValue(Observable.CombineLatest(taxBase, funcGetTaxpercent(), (b, p) => b * p));
            this.adminExpend.SetBaseValue(this.num.obs.Select(x => def.is_collect_tax ? x * 0.0005 : 0));

            if (this.consume != null)
            {
                this.consume.SetBaseValue(new SubjectValue<double>(def.consume.Value).obs);
            }
        }

        [JsonConstructor]
        private Pop()
        {
            
        }
    }
}