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

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Depart
    {
        [JsonProperty, DataVisitorProperty("name")]
        public string name;

        [JsonProperty, DataVisitorProperty("crop_grown")]
        public SubjectValue<double> cropGrown;

        public ObservableValue<int> popNum;

        public ObservableValue<double> tax;
        public ObservableValue<double> adminExpendBase;

        public IEnumerable<Pop> pops
        {
            get
            {
                return GMRoot.runner.pops.Where(x => x.depart_name == name);
            }
        }

        internal Def.Depart def
        {
            get
            {
                return GMRoot.define.departs.Single(x=>x.key == name);
            }
        }

        public static Depart GetByColor(int r, int g, int b)
        {
            return GMRoot.runner.departs.SingleOrDefault(x => x.SameColor((r, g, b)));
        }

        internal static List<Depart> Init(IEnumerable<GMData.Def.Depart> departDefs)
        {
            List<Depart> departs = new List<Depart>();
            foreach(var def in departDefs)
            {
                departs.Add(new Depart(def.key));
            }

            return departs;
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

        internal Depart(string name)
        {
            this.name = name;
            this.cropGrown = new SubjectValue<double>(0);

            foreach (var pop_init in def.pop_init)
            {
                GMRoot.runner.pops.Add(new Pop(name, pop_init.type, pop_init.num));
            }

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Depart()
        {

        }

        [OnDeserialized]
        internal void InitObservableData(StreamingContext context)
        {
            popNum = Observable.CombineLatest(pops.Where(x => x.def.is_collect_tax).Select(x => x.num.obs),
                                  (IList<double> nums) => nums.Sum(y => (int)y)).ToOBSValue();

            tax = Observable.CombineLatest(pops.Select(x => x.tax.value.obs),
                                    (IList<double> nums) => nums.Sum()).ToOBSValue();

            adminExpendBase = Observable.CombineLatest(pops.Select(x => x.adminExpend.value.obs),
                                   (IList<double> nums) => nums.Sum()).ToOBSValue();
        }

        private bool SameColor((int r, int g, int b) p)
        {
            return ((int)def.color.r == p.r && def.color.g == p.g && def.color.b == p.b);
        }
    }
}
