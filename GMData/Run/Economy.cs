﻿using System;
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

        [JsonProperty, DataVisitorProperty("income")]
        public InComes incomes;

        [JsonProperty, DataVisitorProperty("output")]
        public Outputs outputs;

        [DataVisitorProperty("month_surplus")]
        public ObservableValue<double> monthSurplus;


        internal void DaysInc()
        {
            if (GMRoot.runner.date == (null, null, 30))
            {
                curr.Value += monthSurplus.Value;

                outputs.expend();
            }
        }

        internal Economy(Def.Economy init)
        {
            curr = new SubjectValue<double>(init.curr);

            incomes = new InComes(init);
            outputs = new Outputs(init);

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Economy()
        {

        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            monthSurplus = Observable.CombineLatest(incomes.total.obs, outputs.total.obs, (i, o) => i - o).ToOBSValue();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class InComes : IEnumerable<InCome>
    {
        public ObservableValue<double> total;

        [JsonProperty]
        InCome popTax;

        public InComes(Def.Economy init) : this()
        {
            popTax.percent.Value = init.income_percent_pop_tax;

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private InComes()
        {
            InCome.all = new List<InCome>();

            popTax = new InCome("STATIC_POP_TAX",
                                 0,
                                 Observable.CombineLatest(GMRoot.runner.departs.Select(x => x.tax.obs), (IList<double> taxs) => taxs.Sum()).ToOBSValue());
        }

        public IEnumerator<InCome> GetEnumerator()
        {
            foreach(var elem in InCome.all)
            {
                yield return elem;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var elem in InCome.all)
            {
                yield return elem;
            }
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            total = Observable.CombineLatest(InCome.all.Select(x => x.currValue.obs), (IList<double> all) => all.Sum()).ToOBSValue();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Outputs : IEnumerable<Output>
    {
        public ObservableValue<double> total;

        [JsonProperty]
        Output departAdmin;

        [JsonProperty]
        Output reportChaoting;

        public Outputs(Def.Economy init) : this()
        {
            departAdmin.percent.Value = init.output_percent_admin;
            reportChaoting.percent.Value = init.output_percent_chaoting_tax;

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Outputs()
        {
            Output.all = new List<Output>();

            departAdmin = new Output("STATIC_ADMIN_EXPEND",
                                 0,
                                 Observable.CombineLatest(GMRoot.runner.departs.Select(x => x.adminExpendBase.obs), (IList<double> expend) => expend.Sum()).ToOBSValue());

            reportChaoting = new Output("STATIC_REPORT_CHAOTING_TAX",
                                0,
                                GMRoot.runner.chaoting.expectMonthTaxValue);
            reportChaoting.expend = GMRoot.runner.chaoting.ReportMonthTax;
        }

        public IEnumerator<Output> GetEnumerator()
        {
            foreach (var elem in Output.all)
            {
                yield return elem;
            }
        }

        internal void expend()
        {
            foreach (var elem in Output.all)
            {
                elem.expend?.Invoke(elem.currValue.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var elem in Output.all)
            {
                yield return elem;
            }
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            total = Observable.CombineLatest(Output.all.Select(x => x.currValue.obs), (IList<double> all) => all.Sum()).ToOBSValue();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class InCome
    {
        public static List<InCome> all;

        [JsonProperty]
        public string name;

        [JsonProperty]
        public SubjectValue<double> percent;

        public ObservableValue<double> currValue;

        public ObservableValue<double> maxValue;

        internal InCome(string name, double percent, ObservableValue<double> maxValue) : this()
        {
            this.name = name;
            this.percent = new SubjectValue<double>(percent);
            this.maxValue = maxValue;

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private InCome()
        {
            all.Add(this);
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            this.currValue = Observable.CombineLatest(this.percent.obs, maxValue.obs, (p, m) => p * m / 100).ToOBSValue();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Output
    {
        public static List<Output> all;

        [JsonProperty]
        public string name;

        [JsonProperty]
        public SubjectValue<double> percent;

        public ObservableValue<double> currValue;

        public ObservableValue<double> maxValue;

        public Action<double> expend;

        internal Output(string name, double percent, ObservableValue<double> maxValue, Action<double> expend = null) : this()
        {
            this.name = name;
            this.percent = new SubjectValue<double>(percent);
            this.maxValue = maxValue;
            this.expend = expend;

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Output()
        {
            all.Add(this);
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            this.currValue = Observable.CombineLatest(this.percent.obs, maxValue.obs, (p, m) => p * m / 100).ToOBSValue();
        }
    }
}
