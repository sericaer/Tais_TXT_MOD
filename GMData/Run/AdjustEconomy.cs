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
    public class AdjustEconomy
    {
        [JsonProperty]
        public List<IncomAdjust> incomeAdjusts;

        [JsonProperty]
        public List<OutputAdjust> outputAdjusts;

        public AdjustEconomy(GMData.Def.Economy economy)
        {
            incomeAdjusts = economy.incomes.Select(def => new IncomAdjust(def.key)).ToList();

            outputAdjusts = economy.outputs.Select(def => new OutputAdjust(def.key)).ToList();

        }

        [JsonConstructor]
        private AdjustEconomy()
        {
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class IncomAdjust
    {
        [JsonProperty]
        public string key;

        [JsonProperty]
        public SubjectValue<bool> valid;

        [JsonProperty]
        public SubjectValue<int> level;

        public ObservableValueEx<double> percent;
        public ObservableValueEx<double?> effect_pop_consume;

        internal Def.IncomeAdjust def => GMRoot.define.economy.incomes.Single(x => x.key == key);

        [JsonConstructor]
        private IncomAdjust()
        {
            valid = new SubjectValue<bool>(true);
            level = new SubjectValue<int>(0);

            percent = new ObservableValueEx<double>();
            effect_pop_consume = new ObservableValueEx<double?>();
        }

        public IncomAdjust(string key) : this()
        {
            this.key = key;

            this.valid.Value = def.valid;
            this.level.Value = def.init_level;

            DataReactive(new StreamingContext());
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            level.Subscribe(x =>
            {
                percent.OnNext(def.levels[x - 1].percent);
                effect_pop_consume.OnNext(def.levels[x-1].effect_pop_consume);
            });
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class OutputAdjust
    {
        [JsonProperty]
        public string key;

        [JsonProperty]
        public SubjectValue<bool> valid;

        [JsonProperty]
        public SubjectValue<int> level;

        public ObservableValueEx<double> percent;
        //public ObservableValueEx<double?> effect_report_chaoting;
        //public ObservableValueEx<double?> effect_spend_admin;

        internal Def.OutputAdjust def => GMRoot.define.economy.outputs.Single(x => x.key == key);

        [JsonConstructor]
        private OutputAdjust()
        {
            valid = new SubjectValue<bool>(true);
            level = new SubjectValue<int>(0);

            percent = new ObservableValueEx<double>();
        }

        public OutputAdjust(string key) : this()
        {
            this.key = key;
            this.valid.Value = def.valid;
            this.level.Value = def.init_level;

            DataReactive(new StreamingContext());
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            level.Subscribe(x =>
            {
                percent.OnNext(def.levels[x - 1].percent);
            });
        }
    }
}
