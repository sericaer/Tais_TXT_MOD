using DataVisit;
using GMData;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using DynamicData;
using ReactiveMarbles.PropertyChanged;
using GMData.Mod;
using ImpromptuInterface;
using Dynamitey;

namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestRisk : TestRunDataBase
    {
        private GMData.Run.Risks risks;

        private RISK_DEF_MOCK def1 = new RISK_DEF_MOCK
        {
            key = "TEST_RISK_1",
            cost_days = 200,
            options = new GMData.Def.IChoice[]
            {
                new
                {
                    desc = new
                    {
                        name = "TEST_RISK_1_OPTION1",
                        format = "TEST_RISK_1_OPTION1",
                        argv = new string[]{"12"}

                    }.ActLike<GMData.Def.IDesc>()

                }.ActLike<GMData.Def.IChoice>(),

                new
                {
                    desc = new
                    {
                        name = "TEST_RISK_1_OPTION2",
                        format = "TEST_RISK_1_OPTION2",
                        argv = new string[]{"12"}

                    }.ActLike<GMData.Def.IDesc>()

                }.ActLike<GMData.Def.IChoice>(),
            },

            CalcEndEvent = (risk)=>
            {
                return new GEvent_MOCK()
                {
                    key = "EVENT_END_TEST_RISK_1",
                    objTuple = new Tuple<string, object>("risk", risk)
                };
            },
        };

        private RISK_DEF_MOCK def2 = new RISK_DEF_MOCK
        {
            key = "TEST_RISK_2",
            cost_days = 100,
        };

        private RISK_DEF_MOCK def3 = new RISK_DEF_MOCK
        {
            key = "TEST_RISK_3",
            cost_days = 100000,

            options = new GMData.Def.IChoice[]
            {
                new
                {
                    desc = new
                    {
                        name = "TEST_RISK_3_OPTION1"

                    }.ActLike<GMData.Def.IDesc>(),

                    CalcRandomEvent = Return<IGEvent>.Arguments<object>(x=> {
                        var risk = x as GMData.Run.Risk;
                        return new
                        {
                            key = $"EVENT_RANDOM_{risk.key}"
                        }.ActLike<IGEvent>();
                    })
                }.ActLike<GMData.Def.IChoice>()
            }
        };


        [SetUp]
        public void Init()
        {
            risks = new GMData.Run.Risks();

            GMData.Run.Risk.funcGetDef = (key) =>
            {
                if(key == def1.key)
                {
                    return def1;
                }
                if (key == def2.key)
                {
                    return def2;
                }
                if (key == def3.key)
                {
                    return def3;
                }
                return null;
            };
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(0, risks.Items.Count());
        }

        [Test()]
        public void Test_Start()
        {

            GMData.Run.Risk riskAdd = null;
            GMData.Run.Risk riskRemove = null;

            risks.Connect().OnItemAdded(x => riskAdd = x).Subscribe();
            risks.Connect().OnItemRemoved(x => riskRemove = x).Subscribe();

            risks.start = def1.key;
            Assert.AreEqual(def1.key, riskAdd.key);
            Assert.AreEqual(0, riskAdd.percent);

            risks.start = def2.key;
            Assert.AreEqual(def2.key, riskAdd.key);
            Assert.AreEqual(0, riskAdd.percent);

            Assert.AreEqual(2, risks.Count);

            var risk1 = risks.Items.First();
            Assert.AreEqual("TEST_RISK_1", risk1.key);
            Assert.AreEqual(0, risk1.selectedChoices.Items.Count());
            Assert.AreEqual(def1, risk1.def);

            var risk2 = risks.Items.Last();
            Assert.AreEqual("TEST_RISK_2", risk2.key);
            Assert.AreEqual(0, risk2.selectedChoices.Items.Count());
            Assert.AreEqual(def2, risk2.def);
        }

        [Test()]
        public void Test_SelectChoice()
        {
            risks.start = def1.key;

            var risk = risks.Items.First();
            Assert.AreEqual(0, risk.selectedChoices.Count);

            string addOpt = "";
            risk.selectedChoices.Connect().OnItemAdded(x => addOpt = x).Subscribe();

            risk.SelectChoice(1);
            Assert.AreEqual("TEST_RISK_1_OPTION2", addOpt);
            Assert.AreEqual(1, risk.selectedChoices.Count);
        }

        [Test()]
        public void Test_RandomEvent()
        {
            risks.start = def3.key;

            var risk = risks.Items.First();
            Assert.AreEqual(0, risk.selectedChoices.Count);

            string addOpt = "";
            risk.selectedChoices.Connect().OnItemAdded(x => addOpt = x).Subscribe();

            risk.SelectChoice(0);

            var randomEvents = risks.DaysInc();
            Assert.AreEqual(1, randomEvents.Count());
            Assert.AreEqual($"EVENT_RANDOM_{risk.key}", randomEvents.First().key);
        }

        [Test()]
        public void Test_DayInc()
        {
            risks.start = def1.key;
            risks.start = def2.key;

            TestRun();
        }

        private void TestRun()
        {

            GMData.Run.Risk removed = null;
            risks.Connect().OnItemRemoved(x => removed = x).Subscribe();

            decimal percentRisk0 = 0;
            decimal percentRisk1 = 0;

            risks.elems.First().WhenPropertyValueChanges(x => x.percent).Subscribe(x => percentRisk0 = x);
            risks.elems.Last().WhenPropertyValueChanges(x => x.percent).Subscribe(x => percentRisk1 = x);

            for (int i = 1; i <= Math.Max(def1.cost_days, def2.cost_days); i++)
            {
                IGEvent currEvent = null;
                foreach (var eventDef in risks.DaysInc())
                {
                    currEvent = eventDef;
                }

                if (i == def2.cost_days)
                {

                }

                if (i <= def1.cost_days)
                {
                    Assert.AreEqual(i * 100 / def1.cost_days, percentRisk0);
                }

                if (i <= def2.cost_days)
                {
                    Assert.AreEqual(i * 100 / def2.cost_days, percentRisk1);
                }

                if (def1.cost_days == i)
                {
                    Assert.AreEqual("EVENT_END_TEST_RISK_1", currEvent.key);
                    Assert.AreEqual(def1.key, ((GMData.Run.Risk)currEvent.objTuple.Item2).key);

                    Assert.AreEqual(def1.key, removed.key);
                    Assert.AreEqual(100, removed.percent);
                    Assert.AreEqual(true, removed.isEnd);
                }
                if (def2.cost_days == i)
                {
                    Assert.AreEqual(def2.key, removed.key);
                    Assert.AreEqual(100, removed.percent);
                    Assert.AreEqual(true, removed.isEnd);
                }
            }

            Assert.AreEqual(0, risks.Count);
        }

        [Test()]
        public void Test_Serialize()
        {
            risks.start = def1.key;
            risks.start = def2.key;

            var json = JsonConvert.SerializeObject(risks);
            risks = JsonConvert.DeserializeObject<GMData.Run.Risks>(json);

            TestRun();
        }
    }
}
