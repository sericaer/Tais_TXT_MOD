using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DataVisit;
using Newtonsoft.Json;
using DynamicData;
using ReactiveMarbles.PropertyChanged;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Depart
    {
        [JsonProperty, DataVisitorProperty("name")]
        public string name;

        [JsonProperty, DataVisitorProperty("crop_grown")]
        public SubjectValue<double> cropGrown;

        public OBSValue<int> popNum;

        public OBSValue<decimal> tax;
        public OBSValue<decimal> adminExpend;

        [JsonProperty]
        public Pop[] pops
        {
            get
            {
                return _pops.ToArray();
            }
            set
            {
                _pops.AddRange(value);
                _pops.ForEach(x=>x.depart = this);
            }
        }

        private List<Pop> _pops = new List<Pop>();

        internal Def.Depart def => GMRoot.define.departs.Single(x=>x.key == name);

        public static Depart GetByColor(int r, int g, int b)
        {
            return GMRoot.runner.departs.SingleOrDefault(x => x.SameColor((r, g, b)));
        }

        internal static void DaysInc()
        {
            //all.ForEach(x =>
            //{
            //    if (Date.inst >= Root.def.crop.growStartDay && Date.inst <= Root.def.crop.harvestDay)
            //    {
            //        x.cropGrown.Value += Root.def.crop.growSpeed;
            //    }
            //    else
            //    {
            //        x.cropGrown.Value = 0;
            //    }
            //});

        }

        internal Depart(GMData.Def.Depart def) : this()
        {
            this.name = def.key;

            pops = def.pop_init.Select(pop_def => new Pop(this, pop_def.type, pop_def.num, pop_def.party)).ToArray();

            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Depart()
        {
            this.cropGrown = new SubjectValue<double>(0);

            this.popNum = new OBSValue<int>();
            this.tax = new OBSValue<decimal>();
            this.adminExpend = new OBSValue<decimal>();
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            pops.Where(x => x.def.is_collect_tax).Select(x => x.OBSProperty(y=>y.num)).CombineLatest(all => (int)all.Sum())
                .Subscribe(popNum);


            pops.CombineLatestSum(x => x.tax?.OBSProperty(z=>z.value))
                .Subscribe(tax);

            pops.CombineLatestSum(x => x.adminExpend?.OBSProperty(z => z.value))
                .Subscribe(adminExpend);
                
        }

        private bool SameColor((int r, int g, int b) p)
        {
            return ((int)def.color.r == p.r && def.color.g == p.g && def.color.b == p.b);
        }
    }
}
