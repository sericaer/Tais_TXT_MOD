using DataVisit;
using GMData;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestDepart : TestRunDataBase
    {
        private GMData.Def.Depart def;
        private GMData.Run.Depart depart;

        [SetUp]
        public void Init()
        {
            def = GMRoot.define.departs.First();
            depart = new GMData.Run.Depart(def);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(def.pop_init.Count(), depart.pops.Count());
            foreach (var pop_init in def.pop_init)
            {
                var pop = depart.pops.SingleOrDefault(x => x.name == pop_init.type);
                Assert.NotNull(pop);

                Assert.AreEqual(pop_init.num, pop.num.Value);
            }

            Assert.AreEqual(depart.pops.Sum(x => x.tax?.value), depart.tax.Value);
            Assert.AreEqual(depart.pops.Sum(x => x.adminExpend?.value), depart.adminExpend.Value);
        }

        //[Test()]
        //public void TestGetByColor()
        //{
        //    var departObj = GMData.Run.Depart.GetByColor((int)def.color.r, (int)def.color.g, (int)def.color.b);

        //    Assert.AreEqual(depart, departObj);
        //}

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(depart, Formatting.Indented);
            depart = JsonConvert.DeserializeObject<GMData.Run.Depart>(json);
            Test_Init();
        }
    }
}
