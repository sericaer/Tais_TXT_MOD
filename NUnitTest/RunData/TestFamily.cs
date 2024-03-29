﻿using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestFamily : TestRunDataBase
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

                Assert.NotNull(family);
                Assert.AreEqual(pop, family.pop);

                var personNum = pop.num / 150;
                Assert.AreEqual(personNum > 3? personNum: 3, family.persons.Count());

                foreach(var p in family.persons)
                {
                    var buff = family.relation.buffers.Single(x => x.key == p.fullName);
                    Assert.AreEqual(p.relation.value, buff.value);
                }

                Assert.AreEqual(family.relation.value, family.relation.buffers.Sum(x=>x.value) / family.relation.buffers.Count());
            }
        }

        [Test()]
        public void Test_AddPerson()
        {
            var family = depart.pops.SelectNotNull(x => x.family).First();

            var person = family.GeneratePerson();
            Assert.True(family.persons.Contains(person));

            person.relation.SetBuffer("TEST_R", 11);

            var buff = family.relation.buffers.Single(x => x.key == person.fullName);
            Assert.AreEqual(person.relation.value, buff.value);

            Assert.AreEqual(family.relation.value, family.relation.buffers.Sum(x => x.value) / family.relation.buffers.Count());
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
