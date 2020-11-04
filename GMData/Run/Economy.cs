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

        public List<IncomeDetail> incomeDetails;

        public List<OutputDetail> outputDetails;

        [DataVisitorProperty("output_total")]
        public ObservableValueEx<double> outputTotal;

        [DataVisitorProperty("income_total")]
        public ObservableValueEx<double> incomeTotal;

        [DataVisitorProperty("month_surplus")]
        public ObservableValueEx<double> monthSurplus;

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

            incomeDetails = new List<IncomeDetail>();
            foreach(IncomeDetail.TYPE elem in typeof(IncomeDetail.TYPE).GetEnumValues())
            {
                incomeDetails.Add(new IncomeDetail(elem));
            }
            
            outputDetails = new List<OutputDetail>();
            foreach (OutputDetail.TYPE elem in typeof(OutputDetail.TYPE).GetEnumValues())
            {
                outputDetails.Add(new OutputDetail(elem));
            }

            outputTotal = new ObservableValueEx<double>();
            incomeTotal = new ObservableValueEx<double>();
            monthSurplus = new ObservableValueEx<double>();
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            outputDetails.Select(x => x.Value.obs).CombineLatest()
                   .Subscribe(os=> outputTotal.OnNext(os.Sum()));

            incomeDetails.Select(x => x.Value.obs).CombineLatest()
                   .Subscribe(os => incomeTotal.OnNext(os.Sum()));

            Observable.CombineLatest(incomeTotal.obs, outputTotal.obs, (i, o) => i - o)
                      .Subscribe(x=> monthSurplus.OnNext(x));
        }
    }

    public class IncomeDetail
    {
        public enum TYPE
        {
            POP_TAX
        }

        public TYPE type;
        public ObservableValueEx<double> Value;

        public IncomeDetail(TYPE elem)
        {
            this.type = elem;
            this.Value = new ObservableValueEx<double>();
        }
    }

    public class OutputDetail
    {
        public enum TYPE
        {
            ADMIN,
            CHAOTING,
        }

        public TYPE type;
        public ObservableValueEx<double> Value;

        public OutputDetail(TYPE elem)
        {
            this.type = elem;
            this.Value = new ObservableValueEx<double>();
        }
    }
}
