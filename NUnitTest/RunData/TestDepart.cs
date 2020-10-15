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
            GMRoot.runner = new GMData.Run.Runner();
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
                    var pop = depart.pops.SingleOrDefault(x => x.name == pop_init.name);
                    Assert.NotNull(pop);

                    Assert.AreEqual(pop_init.num, pop.num.Value);
                }
            }
        }
    }
}
