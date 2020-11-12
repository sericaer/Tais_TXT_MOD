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
    public class TestAllLink
    {
        [SetUp]
        public void Init()
        {
            GMRoot.Init();
            GMRoot.modder = new GMData.Mod.Modder(TestContext.CurrentContext.TestDirectory + "/../../../Tais_godot/Release/Tais/mods/");
            GMRoot.initData = new GMData.Init.InitData()
            {
                start_date = new GMData.Init.Date()
                {
                    day = 1,
                    month = 1,
                    year = 1
                },

                taishou = new GMData.Init.Taishou()
                {
                    name = "TEST_NAME",
                    age = 30,
                    party = GMRoot.modder.parties.First().key
                }
            };

            GMRoot.runner = new GMData.Run.Runner(GMRoot.initData);
        }

        [Test()]
        public void TestInit()
        {
            var persons = GMRoot.runner.persons;
            Assert.NotZero(persons.Count);

            foreach (var person in persons)
            {
                Assert.True(person.relation.buffers.Keys.Contains("PARTY_RELATION"));

                var partyDef = GMRoot.define.parties.Single(x => x.key == person.family.partyName);
                Assert.AreEqual(partyDef.relation.Single(x => x.peer == GMRoot.runner.taishou.partyName).value, person.relation.buffers.Lookup("PARTY_RELATION").Value.value);
            }
        }

        [Test()]
        public void TestRun()
        {
            for(int i=0; i<360*10; i++)
            {
                GMRoot.runner.DaysInc();

                foreach (var eventobj in GMRoot.modder.events)
                {
                    Console.WriteLine($"event {eventobj.title} start");
                }

                foreach (var warn in GMRoot.modder.warns)
                {
                    Console.WriteLine($"warn {warn.key} start");
                }
            }
            
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = GMRoot.runner.Serialize();
            GMRoot.runner = GMData.Run.Runner.Deserialize(json);

            TestRun();
        }
    }
}
