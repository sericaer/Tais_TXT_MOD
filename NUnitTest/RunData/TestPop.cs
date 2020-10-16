using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestPop : TestRunDataBase
    {
        [SetUp]
        public void Init()
        {
            GMRoot.runner = new GMData.Run.Runner();
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.define.departs.SelectMany(x => x.pop_init).Count(), GMRoot.runner.pops.Count);

            foreach (var departDef in GMRoot.define.departs)
            {
                foreach(var pop_init in departDef.pop_init)
                {
                    var pop = GMRoot.runner.pops.SingleOrDefault(x => (x.name == pop_init.type && x.depart.name == departDef.key));
                    Assert.AreEqual(pop_init.num, pop.num.Value);

                    var popDef = GMRoot.define.pops.SingleOrDefault(x => x.key == pop.name);
                    Assert.AreEqual(popDef, pop.def);
                }
                
            }
        }
    }
}
