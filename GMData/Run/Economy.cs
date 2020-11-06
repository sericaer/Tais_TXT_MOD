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
        public SubjectValue<double> curr;

        [DataVisitorProperty("output_total")]
        public OBSValue<double> outputTotal;

        [DataVisitorProperty("income_total")]
        public OBSValue<double> incomeTotal;

        [DataVisitorProperty("month_surplus")]
        public OBSValue<double> monthSurplus;

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
            curr.Value = init.curr;
            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Economy()
        {
            curr = new SubjectValue<double>(0);
            detail = new Detail();

            outputTotal = new OBSValue<double>();
            incomeTotal = new OBSValue<double>();
            monthSurplus = new OBSValue<double>();
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
            internal OBSValue<double> popTax;

            internal OBSValue<double> reportChaoting;
            internal OBSValue<double> adminSpend;

            internal IEnumerable<OBSValue<double>> incomeDetails
            {
                get
                {
                    yield return popTax;
                }
            }

            internal IEnumerable<OBSValue<double>> outputDetails
            {
                get
                {
                    yield return reportChaoting;
                    yield return adminSpend;
                }
            }

            internal Detail()
            {
                popTax = new OBSValue<double>();
                reportChaoting = new OBSValue<double>();
                adminSpend = new OBSValue<double>();
            }
        }
    }
}
