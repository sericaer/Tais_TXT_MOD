using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Economy : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty, DataVisitorProperty("value")]
        public decimal curr { get; set; }

        [DataVisitorProperty("output_total")]
        public decimal outputTotal { get; set; }

        [DataVisitorProperty("income_total")]
        public decimal incomeTotal { get; set; }

        [DataVisitorProperty("month_surplus")]
        public decimal monthSurplus { get; set; }

        public Detail detail;

        internal void DaysInc(Date date)
        {
            if (date == (null, null, 30))
            {
                curr += monthSurplus;
            }
        }

        internal Economy(Def.Economy init) : this()
        {
            curr = (decimal)init.curr;
            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Economy()
        {
            detail = new Detail();
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            detail.incomeDetails.CombineLatest(all => all.Sum())
                   .Subscribe(x=> incomeTotal = x);

            detail.outputDetails.CombineLatest(all => all.Sum())
                   .Subscribe(x=> outputTotal = x);

            Observable.CombineLatest(this.OBSProperty(x=>x.incomeTotal), this.OBSProperty(x=>x.outputTotal), (i, o) => i - o)
                      .Subscribe(x=> monthSurplus = x);
        }

        public class Detail : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public decimal popTax { get; set; }

            public decimal reportChaoting { get; set; }
            public decimal adminSpend { get; set; }

            public IEnumerable<IObservable<decimal>> incomeDetails
            {
                get
                {
                    yield return this.OBSProperty(x=>x.popTax);
                }
            }

            public IEnumerable<IObservable<decimal>> outputDetails
            {
                get
                {
                    yield return this.OBSProperty(x => x.reportChaoting);
                    yield return this.OBSProperty(x => x.adminSpend);
                }
            }

            public Detail()
            {
            }
        }
    }
}
