using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestDepart : TestRunDataBase
    {
        [SetUp]
        public void Init()
        {
            GMRoot.runner = GMData.Run.Runner.Generate();
        }

        [Test()]
        public void Test_Init()
        {
            int i = 0;
            Visitor.Pos pos = null;
            while (Visitor.EnumerateVisit("depart", ref pos))
            {
                var def = GMRoot.define.departs[i];
                Assert.AreEqual(def.key, Visitor.Get("depart.name"));
                Assert.AreEqual(0, Visitor.Get("depart.crop_grown"));
                i++;
            }

            foreach(var def in GMRoot.define.departs)
            {
                var depart = GMRoot.runner.departs.SingleOrDefault(x => x.name == def.key);
                Assert.NotNull(depart);

                Assert.AreEqual(def.pop_init.Count(), depart.pops.Count());
                foreach(var pop_init in def.pop_init)
                {
                    var pop = depart.pops.SingleOrDefault(x => x.name == pop_init.type);
                    Assert.NotNull(pop);

                    Assert.AreEqual(pop_init.num, pop.num.Value);
                }

                Assert.AreEqual(depart.pops.Sum(x=>x.tax.value.Value), depart.tax.Value);
                Assert.AreEqual(depart.pops.Sum(x => x.adminExpend.value.Value), depart.adminExpend.Value);
            }
        }

        [Test()]
        public void TestGetByColor()
        {
            var departDef0 = GMRoot.define.departs[0];
            var departObj0 = GMData.Run.Depart.GetByColor((int)departDef0.color.r, (int)departDef0.color.g, (int)departDef0.color.b);

            Assert.AreEqual(departDef0, departObj0.def);

            var departDef1 = GMRoot.define.departs[1];
            var departObj1 = GMData.Run.Depart.GetByColor((int)departDef1.color.r, (int)departDef1.color.g, (int)departDef1.color.b);

            Assert.AreEqual(departDef1, departObj1.def);
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = GMRoot.runner.Serialize();
            GMRoot.runner = GMData.Run.Runner.Deserialize(json);

            Test_Init();
        }
    }
}
