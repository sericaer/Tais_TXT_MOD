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
    public class TestRisk : TestRunDataBase
    {
        private GMData.Run.Risk risk;
        private GMData.Def.Risk def;

        [SetUp]
        public void Init()
        {
            def = GMRoot.define.risks[0];
            risk = new GMData.Run.Risk(def.key);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(0, risk.percent.Value);
            Assert.AreEqual(def.key, risk.key);
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(risk);

            risk = JsonConvert.DeserializeObject<GMData.Run.Risk>(json);
            Test_Init();
        }

        [Test()]
        public void Test_RiskDaysInc()
        {
            double percent = 0.0;
            Assert.AreEqual(percent, risk.percent.Value);

            for (int i = 1; i <= def.cost_days; i++)
            {
                risk.DaysInc();

                percent += (100 / def.cost_days);
                Assert.AreEqual(percent, risk.percent.Value);
            }
        }
    }
}
