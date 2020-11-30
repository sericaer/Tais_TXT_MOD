using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;
using ReactiveMarbles.PropertyChanged;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Runner
    {
        [JsonProperty, DataVisitorProperty("date")]
        public Date date;

        [JsonProperty, DataVisitorProperty("economy")]
        public Economy economy;

        [JsonProperty, DataVisitorProperty("taishou")]
        public Taishou taishou;

        [JsonProperty, DataVisitorPropertyArray("depart")]
        public List<Depart> departs;

        [JsonProperty, DataVisitorProperty("chaoting")]
        public Chaoting chaoting;

        [DataVisitorPropertyArray("pop")]
        public List<Pop> pops => departs.SelectMany(x => x.pops).ToList();

        [DataVisitorPropertyArray("family")]
        public List<Family> families => pops.SelectNotNull(x => x.family).ToList();

        [DataVisitorPropertyArray("person")]
        public List<Person> persons => families.SelectMany(x => x.persons).ToList();

        [JsonProperty, DataVisitorPropertyArray("party")]
        public List<Party> parties;

        public IObservable<decimal> registerPopNum;

        [JsonProperty, DataVisitorProperty("risk")]
        public Risks _risks;

        [JsonProperty]
        public List<Adjust> adjusts;

        public static Runner Generate()
        {
            var runner = new Runner(GMRoot.initData);
            return runner;
        }

        public static Runner Deserialize(string json)
        {
            var obj = JsonConvert.DeserializeObject<Runner>(json);
            return obj;
        }

        public Runner(Init.InitData init) : this()
        {
            date = new Date(init.start_date);

            taishou = new Taishou(init.taishou);

            parties = GMRoot.define.parties.Select(def => new Party(def)).ToList();

            departs = GMRoot.define.departs.Select(def => new Depart(def)).ToList();

            chaoting = new Chaoting(GMRoot.define.chaoting, pops.Where(x => x.def.is_collect_tax).Sum(x => x.num));

            economy = new Economy(GMRoot.define.economy);

            adjusts = Enum.GetValues(typeof(Adjust.EType)).Cast<Adjust.EType>().Select(x => new Adjust(x)).ToList();

            _risks = new Risks();

            var names = GMRoot.define.personName.GetRandomFamilyArray(families.Count);
            for (int i = 0; i < families.Count; i++)
            {
                families[i].name = names[i];
            }

            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Runner()
        {
            Visitor.SetVisitData(this);
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            adjusts.ForEach(ad =>
            {
                ad.OBSProperty(x=>x.level).Subscribe(level =>
                {
                    switch (ad.etype)
                    {
                        case Adjust.EType.POP_TAX:
                            foreach (var tax in pops.SelectNotNull(pop => pop.tax))
                            {
                                tax.SetBuffer(ad.etype.ToString(), (decimal)ad.levelDef.percent * tax.baseValue * 0.01M);
                            }
                            break;
                        case Adjust.EType.ADMIN_SPEND:
                            foreach (var admin in pops.SelectNotNull(pop => pop.adminExpend))
                            {
                                admin.SetBuffer(ad.etype.ToString(), (decimal)ad.levelDef.percent * admin.baseValue * 0.01M);
                            }
                            break;
                        case Adjust.EType.REPORT_CHAOTING:
                            chaoting.reportTaxLevel= level;
                            break;
                    }

                    if (ad.levelDef.effect_pop_consume != null)
                    {
                        foreach (var consume in pops.SelectNotNull(pop => pop.consume))
                        {
                            consume.SetBuffer("POP_TAX", (decimal)ad.levelDef.effect_pop_consume * consume.baseValue * 0.01M);
                        }
                    }
                });
            });

            pops.SelectNotNull(pop => pop.tax).ToOBSPropertyList(tax => tax.value)
                .Subscribe(all => economy.detail.popTax = all.Sum());

            pops.SelectNotNull(pop => pop.adminExpend).ToOBSPropertyList(adm => adm.value)
                .Subscribe(all => economy.detail.adminSpend = all.Sum());

            registerPopNum = departs.CombineLatestSum(depart => depart.OBSProperty(x => x.popNum));

            chaoting.OBSProperty(x=>x.monthTaxReqort).Subscribe(x=> economy.detail.reportChaoting = x);

            persons.ForEach(p =>
            {
                var party = parties.Single(x => x.key == p.family.partyName);
                p.relation.SetBuffer("PARTY_RELATION", party.getRelation(taishou.partyName));
            });
        }

        public IEnumerable<GMData.Mod.IGEvent> DaysInc()
        {
            economy.DaysInc(date);

            date.Inc();

            foreach(var eventObj in _risks.DaysInc())
            {
                yield return eventObj;
            }
        }

        public bool isEnd()
        {
            return taishou.isRevoke;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
