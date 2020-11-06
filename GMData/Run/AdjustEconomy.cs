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
        public enum Type
        {
            POP_TAX,
        }

        [JsonProperty]
        public SubjectValue<int> popTaxLevel;

        [JsonProperty]
        public List<OutputAdjust> outputAdjusts;

        public AdjustEconomy(GMData.Def.Economy economy)
        {
            popTaxLevel = new SubjectValue<int>(economy.adjust_pop_tax.init_level);

            outputAdjusts = economy.outputs.Select(def => new OutputAdjust(def.key)).ToList();

        }

        [JsonConstructor]
        private AdjustEconomy()
        {
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

        public OBSValue<double> percent;
        //public ObservableValueEx<double?> effect_report_chaoting;
        //public ObservableValueEx<double?> effect_spend_admin;

        internal Def.OutputAdjust def => GMRoot.define.economy.outputs.Single(x => x.key == key);

        [JsonConstructor]
        private OutputAdjust()
        {
            valid = new SubjectValue<bool>(true);
            level = new SubjectValue<int>(0);

            percent = new OBSValue<double>();
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
