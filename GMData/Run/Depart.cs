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
using System.ComponentModel;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Depart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty, DataVisitorProperty("name")]
        public string name;

        [JsonProperty, DataVisitorProperty("crop_grown")]
        public decimal cropGrown { get; set; }

        public decimal popNum { get; set; }

        public decimal tax { get; set; }
        public decimal adminExpend { get; set; }

        [JsonProperty]
        public IEnumerable<Pop> pops
        {
            get
            {
                return _pops.Items;
            }
            set
            {
                _pops.AddRange(value);
                _pops.Items.ToList().ForEach(x=>x.depart = this);
            }
        }

        private SourceList<Pop> _pops = new SourceList<Pop>();

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

        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            pops.Where(x => x.def.is_collect_tax).Select(x => x.OBSProperty(y=>y.num)).CombineLatest(all => (int)all.Sum())
                .Subscribe(x=>popNum=x);

            pops.CombineLatestSum(x => x.tax?.OBSProperty(z=>z.value))
                .Subscribe(x=>tax=x);

            pops.CombineLatestSum(x => x.adminExpend?.OBSProperty(z => z.value))
                .Subscribe(x=>adminExpend =x);
                
        }

        private bool SameColor((int r, int g, int b) p)
        {
            return ((int)def.color.r == p.r && def.color.g == p.g && def.color.b == p.b);
        }
    }
}
