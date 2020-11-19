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
    public class TestPerson : TestRunDataBase
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

            Assert.AreEqual(def_depart.pop_init.Count(), depart.pops.Count());

            foreach (var pop_init in def_depart.pop_init.Where(x=> GMRoot.define.pops.Single(y=>y.key == x.type).is_family))
            {
                var pop = depart.pops.SingleOrDefault(x => (x.name == pop_init.type && x.depart.name == def_depart.key));
                var family = pop.family;

                foreach(var person in family.persons)
                {

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
