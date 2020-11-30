using DataVisit;
using GMData;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using ReactiveMarbles.PropertyChanged;
using GMData.Mod;

namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestRisk : TestRunDataBase
    {
        //private GMData.Run.Risk risk;
        //private GMData.Def.Risk def;

        //[SetUp]
        //public void Init()
        //{
        //    def = GMRoot.define.risks[0];
        //    risk = new GMData.Run.Risk(def.key);
        //}

        //[Test()]
        //public void Test_Init()
        //{
        //    Assert.AreEqual(0, risk.percent);
        //    Assert.AreEqual(def.key, risk.key);
        //}

        //[Test()]
        //public void Test_Serialize()
        //{
        //    var json = JsonConvert.SerializeObject(risk);

        //    risk = JsonConvert.DeserializeObject<GMData.Run.Risk>(json);
        //    Test_Init();
        //}

        //[Test()]
        //public void Test_RiskDaysInc()
        //{
        //    double percent = 0.0;
        //    Assert.AreEqual(percent, risk.percent);

        //    for (int i = 1; i <= def.cost_days; i++)
        //    {
        //        foreach(var n in risk.DaysInc())
        //        {

        //        }

        //        percent += (100 / def.cost_days);
        //        Assert.AreEqual(percent, risk.percent);
        //    }
        //}

        

        private GMData.Run.Risks risks;

        private RISK_DEF_MOCK def0 = new RISK_DEF_MOCK
        {
            key = "TEST_RISK_0",
            cost_days = 200,

            CalcEndEvent = (risk)=>
            {
                return new GEvent_MOCK()
                {
                    key = "EVENT_END_TEST_RISK_0",
                    objTuple = new Tuple<string, object>("risk", risk)
                };
            }
        };

        private RISK_DEF_MOCK def1 = new RISK_DEF_MOCK
        {
            key = "TEST_RISK_1",
            cost_days = 100,
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
                if (key == def0.key)
                {
                    return def0;
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

            risks.start = def0.key;
            Assert.AreEqual(def0.key, riskAdd.key);
            Assert.AreEqual(0, riskAdd.percent);

            risks.start = def1.key;
            Assert.AreEqual(def1.key, riskAdd.key);
            Assert.AreEqual(0, riskAdd.percent);

            Assert.AreEqual(2, risks.Count);
        }

        [Test()]
        public void Test_DayInc()
        {
            risks.start = def0.key;
            risks.start = def1.key;

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

            for (int i = 1; i <= Math.Max(def0.cost_days, def1.cost_days); i++)
            {
                IGEvent currEvent = null;
                foreach (var eventDef in risks.DaysInc())
                {
                    currEvent = eventDef;
                }

                if (i == def0.cost_days)
                {

                }

                if (i <= def0.cost_days)
                {
                    Assert.AreEqual(i * 100 / def0.cost_days, percentRisk0);
                }

                if (i <= def1.cost_days)
                {
                    Assert.AreEqual(i * 100 / def1.cost_days, percentRisk1);
                }

                if (def0.cost_days == i)
                {
                    Assert.AreEqual("EVENT_END_TEST_RISK_0", currEvent.key);
                    Assert.AreEqual(def0.key, ((GMData.Run.Risk)currEvent.objTuple.Item2).key);

                    Assert.AreEqual(def0.key, removed.key);
                    Assert.AreEqual(100, removed.percent);
                    Assert.AreEqual(true, removed.isEnd);
                }
                if (def1.cost_days == i)
                {
                    Assert.AreEqual(def1.key, removed.key);
                    Assert.AreEqual(100, removed.percent);
                    Assert.AreEqual(true, removed.isEnd);
                }
            }

            Assert.AreEqual(0, risks.Count);
        }

        [Test()]
        public void Test_Serialize()
        {
            risks.start = def0.key;
            risks.start = def1.key;

            var json = JsonConvert.SerializeObject(risks);
            risks = JsonConvert.DeserializeObject<GMData.Run.Risks>(json);

            TestRun();
        }
    }
}
