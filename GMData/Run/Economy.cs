using System;
using System.Collections;
using System.Collections.Generic;
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
    public class Economy
    {
        [JsonProperty, DataVisitorProperty("value")]
        public SubjectValue<decimal> curr;

        [DataVisitorProperty("output_total")]
        public OBSValue<decimal> outputTotal;

        [DataVisitorProperty("income_total")]
        public OBSValue<decimal> incomeTotal;

        [DataVisitorProperty("month_surplus")]
        public OBSValue<decimal> monthSurplus;

        public Detail detail;

        internal void DaysInc(Date date)
        {
            if (date == (null, null, 30))
            {
                curr.Value += monthSurplus.Value;
            }
        }

        internal Economy(Def.Economy init) : this()
        {
            curr.Value = (decimal)init.curr;
            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Economy()
        {
            curr = new SubjectValue<decimal>(0);
            detail = new Detail();

            outputTotal = new OBSValue<decimal>();
            incomeTotal = new OBSValue<decimal>();
            monthSurplus = new OBSValue<decimal>();
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            detail.incomeDetails.CombineLatest(all => all.Sum())
                   .Subscribe(incomeTotal);

            detail.outputDetails.CombineLatest(all => all.Sum())
                   .Subscribe(outputTotal);

            Observable.CombineLatest(incomeTotal, outputTotal, (i, o) => i - o)
                      .Subscribe(monthSurplus);
        }

        public class Detail
        {
            internal OBSValue<decimal> popTax;

            internal OBSValue<decimal> reportChaoting;
            internal OBSValue<decimal> adminSpend;

            internal IEnumerable<OBSValue<decimal>> incomeDetails
            {
                get
                {
                    yield return popTax;
                }
            }

            internal IEnumerable<OBSValue<decimal>> outputDetails
            {
                get
                {
                    yield return reportChaoting;
                    yield return adminSpend;
                }
            }

            internal Detail()
            {
                popTax = new OBSValue<decimal>();
                reportChaoting = new OBSValue<decimal>();
                adminSpend = new OBSValue<decimal>();
            }
        }
    }
}
