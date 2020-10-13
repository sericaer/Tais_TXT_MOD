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
//    public class TestChaoting : TestRunData
//    {
//        [Test()]
//        public void Test_ChaotingExtraTax()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);

//            Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
//            Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));

//            var extraTax = 100.0;
//            Chaoting.inst.ReportMonthTax(Chaoting.inst.expectMonthTaxValue.Value + extraTax);

//            Assert.AreEqual(extraTax, Visitor.Get("chaoting.extra_tax"));
//            Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));
//        }

//        [Test()]
//        public void Test_ChaotingOweTax()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);

//            Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
//            Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));

//            var oweTax = 100.0;
//            Chaoting.inst.ReportMonthTax(Chaoting.inst.expectMonthTaxValue.Value - oweTax);

//            Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
//            Assert.AreEqual(oweTax, Visitor.Get("chaoting.owe_tax"));
//        }

//        [Test()]
//        public void Test_ChaotingPowerParty()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);

//            Assert.AreEqual(Root.def.chaoting.powerParty, Visitor.Get("chaoting.power_party.type"));
//        }
//    }
//}
