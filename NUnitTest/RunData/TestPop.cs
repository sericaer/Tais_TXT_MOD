using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestPop : TestRunDataBase
    {
        private GMData.Def.Depart def_depart;
        private GMData.Run.Depart depart;

        [SetUp]
        public void Init()
        {
            def_depart = GMRoot.define.departs.First();
            depart = new GMData.Run.Depart(def_depart);
        }

        [Test()]
        public void Test_Init()
        {

            Assert.AreEqual(def_depart.pop_init.Count(), depart.pops.Length);

            foreach (var pop_init in def_depart.pop_init)
            {
                var pop = depart.pops.SingleOrDefault(x => (x.name == pop_init.type && x.depart.name == def_depart.key));
                Assert.AreEqual(pop_init.num, pop.num);

                var popDef = GMRoot.define.pops.SingleOrDefault(x => x.key == pop.name);
                Assert.AreEqual(popDef, pop.def);

                if (pop.def.is_collect_tax)
                {
                    Assert.AreEqual(pop.num * 0.01M, pop.tax.value);
                    Assert.AreEqual(pop.num * 0.0005M, pop.adminExpend.value);
                }
                else
                {
                    Assert.Null(pop.tax);
                    Assert.Null(pop.adminExpend);
                }

                if (popDef.consume == null)
                {
                    Assert.Null(pop.consume);
                }
                else
                {
                    Assert.AreEqual(pop.def.consume, pop.consume.value);
                }

                if(popDef.is_family)
                {
                    Assert.NotNull(pop.family);
                }
                else
                {
                    Assert.Null(pop.family);
                }
            }
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(depart, Formatting.Indented);
            depart = JsonConvert.DeserializeObject<GMData.Run.Depart>(json);
            Test_Init();
        }
    }
}
