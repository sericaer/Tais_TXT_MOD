//using DataVisit;
//using Modder;
//using NUnit.Framework;
//using RunData;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//namespace UnitTest.RunData
//{
//    [TestFixture()]
//    public class TestRisk : TestRunData
//    {
//        [Test()]
//        public void Test_RiskStart()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);

//            Assert.AreEqual(0, Risk.all.Count());

//            var def = Root.def.risks[0];
//            Visitor.Set("risk.start", def.key);

//            var risk = Risk.all.SingleOrDefault(x => x.key == def.key);
//            Assert.NotNull(risk);
//            Assert.AreEqual(0, risk.percent.Value);
//        }

//        [Test()]
//        public void Test_RiskDaysInc()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);

//            var def = Root.def.risks[0];

//            var risk = new Risk(def.key);
//            Risk.all.Add(risk);

//            double percent = 0.0;
//            Assert.AreEqual(percent, risk.percent.Value);

//            for (int i=1; i<=def.cost_days; i++)
//            {
//                Risk.DaysInc();

//                percent += (100 / def.cost_days);
//                Assert.AreEqual(percent, risk.percent.Value);
//            }

//            Risk.DaysInc();
//            Assert.AreEqual(0, Risk.all.Count());
//        }
//    }
//}
