using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestRisk : TestRunDataBase
    {
        [SetUp]
        public void Init()
        {
            GMRoot.runner = GMData.Run.Runner.Generate();
        }

        [Test()]
        public void Test_RiskStart()
        {
            Assert.AreEqual(0, GMRoot.runner.risks.Count());

            var def = GMRoot.define.risks[0];
            Visitor.Set("risk.start", def.key);

            var risk = GMRoot.runner.risks.SingleOrDefault(x => x.key == def.key);
            Assert.NotNull(risk);
            Assert.AreEqual(0, risk.percent.Value);
        }

        [Test()]
        public void Test_RiskSerialize()
        {
            Assert.AreEqual(0, GMRoot.runner.risks.Count());

            var def = GMRoot.define.risks[0];
            Visitor.Set("risk.start", def.key);

            var risk = GMRoot.runner.risks.SingleOrDefault(x => x.key == def.key);
            risk.percent.Value = 20;

            var json = GMRoot.runner.Serialize();

            GMRoot.runner = GMData.Run.Runner.Deserialize(json);

            risk = GMRoot.runner.risks.SingleOrDefault(x => x.key == def.key);
            Assert.NotNull(risk);
            Assert.AreEqual(20, risk.percent.Value);
        }

        [Test()]
        public void Test_RiskDaysInc()
        {
            var def = GMRoot.define.risks[0];

            Visitor.Set("risk.start", def.key);

            var risk = GMRoot.runner.risks.SingleOrDefault(x => x.key == def.key);
            Assert.NotNull(risk);

            double percent = 0.0;
            Assert.AreEqual(percent, risk.percent.Value);

            for (int i = 1; i <= def.cost_days; i++)
            {
                GMRoot.runner.risks.DaysInc();

                percent += (100 / def.cost_days);
                Assert.AreEqual(percent, risk.percent.Value);
            }

            GMRoot.runner.risks.DaysInc();
            Assert.AreEqual(0, GMRoot.runner.risks.Count());
        }
    }
}
