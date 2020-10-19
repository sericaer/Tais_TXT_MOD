using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestChaoting : TestRunDataBase
    {
        [SetUp]
        public void Init()
        {
            GMRoot.runner = GMData.Run.Runner.Generate();
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.define.chaoting.powerParty, GMRoot.runner.chaoting.powerParty.name);
            Assert.AreEqual((int)(GMRoot.define.chaoting.reportPopPercent /100 * GMRoot.runner.pops.Where(x=>x.def.is_collect_tax).Sum(x=>x.num.Value)), GMRoot.runner.chaoting.reportPopNum.Value);
            Assert.AreEqual(GMRoot.define.chaoting.taxPercent/100 * 0.006 * GMRoot.runner.chaoting.reportPopNum.Value, GMRoot.runner.chaoting.expectMonthTaxValue.Value);
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = GMRoot.runner.Serialize();
            GMRoot.runner = GMData.Run.Runner.Deserialize(json);

            Test_Init();
        }

        //[Test()]
        //public void Test_ChaotingExtraTax()
        //{
        //    ModDataVisit.InitVisitMap(typeof(Root));

        //    Root.Init(init);
        //    ModDataVisit.InitVisitData(Root.inst);

        //    Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
        //    Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));

        //    var extraTax = 100.0;
        //    Chaoting.inst.ReportMonthTax(Chaoting.inst.expectMonthTaxValue.Value + extraTax);

        //    Assert.AreEqual(extraTax, Visitor.Get("chaoting.extra_tax"));
        //    Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));
        //}

        //[Test()]
        //public void Test_ChaotingOweTax()
        //{
        //    ModDataVisit.InitVisitMap(typeof(Root));

        //    Root.Init(init);
        //    ModDataVisit.InitVisitData(Root.inst);

        //    Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
        //    Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));

        //    var oweTax = 100.0;
        //    Chaoting.inst.ReportMonthTax(Chaoting.inst.expectMonthTaxValue.Value - oweTax);

        //    Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
        //    Assert.AreEqual(oweTax, Visitor.Get("chaoting.owe_tax"));
        //}

        //[Test()]
        //public void Test_ChaotingPowerParty()
        //{
        //    ModDataVisit.InitVisitMap(typeof(Root));

        //    Root.Init(init);
        //    ModDataVisit.InitVisitData(Root.inst);

        //    Assert.AreEqual(Root.def.chaoting.powerParty, Visitor.Get("chaoting.power_party.type"));
        //}
    }
}
