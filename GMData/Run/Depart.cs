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
        public static Func<string, IEnumerable<Pop>> funcGetPop;
        public static Func<string, Def.Depart> funcGetDef;

        [JsonProperty, DataVisitorProperty("name")]
        public string name;

        [JsonProperty, DataVisitorProperty("crop_grown")]
        public SubjectValue<double> cropGrown;

        public ObservableValue<int> popNum;

        public ObservableValue<double> tax;
        public ObservableValue<double> adminExpend;

        public IEnumerable<Pop> pops => funcGetPop(name);

        internal Def.Depart def => funcGetDef(name);

        public static Depart GetByColor(int r, int g, int b)
        {
            return GMRoot.runner.departs.SingleOrDefault(x => x.SameColor((r, g, b)));
        }

        internal static List<Depart> Init(IEnumerable<GMData.Def.Depart> departDefs, out List<Pop> pops)
        {
            pops = new List<Pop>();

            List<Depart> departs = new List<Depart>();
            foreach(var def in departDefs)
            {
                departs.Add(new Depart(def, ref pops));
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

        internal Depart(GMData.Def.Depart def, ref List<Pop> pops)
        {
            this.name = def.key;
            this.cropGrown = new SubjectValue<double>(0);

            foreach (var pop_init in def.pop_init)
            {
                pops.Add(new Pop(name, pop_init.type, pop_init.num));
            }
        }

        [JsonConstructor]
        private Depart()
        {

        }

        internal void DataAssocate()
        {
            popNum = Observable.CombineLatest(pops.Where(x => x.def.is_collect_tax).Select(x => x.num.obs),
                                  (IList<double> nums) => nums.Sum(y => (int)y)).ToOBSValue();

            tax = Observable.CombineLatest(pops.Select(x => x.tax.value.obs),
                                    (IList<double> nums) => nums.Sum()).ToOBSValue();

            adminExpend = Observable.CombineLatest(pops.Select(x => x.adminExpend.value.obs),
                                   (IList<double> nums) => nums.Sum()).ToOBSValue();
        }

        private bool SameColor((int r, int g, int b) p)
        {
            return ((int)def.color.r == p.r && def.color.g == p.g && def.color.b == p.b);
        }
    }
}
