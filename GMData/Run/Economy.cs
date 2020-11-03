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

        [JsonProperty]
        public List<IncomAdjust> incomeAdjusts;

        [JsonProperty]
        public List<OutputAdjust> outputAdjusts;

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

            foreach (var init_income in init.incomes)
            {
                incomeAdjusts.Add(new IncomAdjust(init_income.key));
            }

            foreach (var init_output in init.outputs)
            {
                outputAdjusts.Add(new OutputAdjust(init_output.key));
            }

            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Economy()
        {
            curr = new SubjectValue<double>(0);

            incomeAdjusts = new List<IncomAdjust>();
            outputAdjusts = new List<OutputAdjust>();

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

    [JsonObject(MemberSerialization.OptIn)]
    public class IncomAdjust
    {
        [JsonProperty]
        public string key;

        [JsonProperty]
        public SubjectValue<double> percent;

        public ObservableValueEx<double> effect_pop_tax;
        public ObservableValueEx<double> effect_pop_consume;

        internal Def.IncomeAdjust def => GMRoot.define.economy.incomes.Single(x => x.key == key);

        [JsonConstructor]
        private IncomAdjust()
        {
            percent = new SubjectValue<double>(0);
        }

        public IncomAdjust(string key) : this()
        {
            this.key = key;
            this.percent.Value = def.percent;

            DataReactive(new StreamingContext());
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            if(def.effect_pop_consume != null)
            {
                this.effect_pop_consume = new ObservableValueEx<double>();
                this.percent.Subscribe(x => effect_pop_consume.OnNext(x * def.effect_pop_consume.Value));
            }
            if (def.effect_pop_tax != null)
            {
                this.effect_pop_tax = new ObservableValueEx<double>();
                this.percent.Subscribe(x => effect_pop_tax.OnNext(x * def.effect_pop_tax.Value));
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class OutputAdjust
    {
        [JsonProperty]
        public string key;

        [JsonProperty]
        public SubjectValue<double> percent;

        public ObservableValueEx<double> effect_report_chaoting;
        public ObservableValueEx<double> effect_spend_admin;

        internal Def.OutputAdjust def => GMRoot.define.economy.outputs.Single(x => x.key == key);

        [JsonConstructor]
        private OutputAdjust()
        {
            this.percent = new SubjectValue<double>(0);
        }

        public OutputAdjust(string key) : this()
        {
            this.key = key;
            this.percent.Value = def.percent;

            DataReactive(new StreamingContext());
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            if (def.effect_report_chaoting != null)
            {
                this.effect_report_chaoting = new ObservableValueEx<double>();
                this.percent.Subscribe(x => effect_report_chaoting.OnNext(x * def.effect_report_chaoting.Value));
            }
            if (def.effect_spend_admin != null)
            {
                this.effect_spend_admin = new ObservableValueEx<double>();
                this.percent.Subscribe(x => effect_spend_admin.OnNext(x * def.effect_spend_admin.Value));
            }
        }
    }
}
