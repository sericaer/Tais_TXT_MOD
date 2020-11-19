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
    public class TestChaoting : TestRunDataBase
    {
        private GMData.Run.Chaoting chaoting;

        [SetUp]
        public void Init()
        {
            chaoting = new GMData.Run.Chaoting(GMRoot.define.chaoting, 123456);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.define.chaoting.powerParty, chaoting.powerPartyName);
            Assert.AreEqual((int)(GMRoot.define.chaoting.reportPopPercent /100 * 123456), chaoting.reportPopNum.Value);
            Assert.AreEqual(GMRoot.define.chaoting.tax_level, chaoting.requestTaxLevel.Value);

            var levels = GMRoot.define.adjusts.Single(x => x.key == "REPORT_CHAOTING").levels;
            Assert.AreEqual(chaoting.CalcTax(GMRoot.define.chaoting.tax_level, chaoting.reportPopNum.Value), chaoting.monthTaxRequest.Value);
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(chaoting);
            chaoting = JsonConvert.DeserializeObject<GMData.Run.Chaoting>(json);

            Test_Init();
        }

        [Test()]
        public void Test_ChaotingExtraTax()
        {
            Assert.AreEqual(0, chaoting.extraTax);

            var extraTax = 100.0M;
            chaoting.ReportMonthTax(chaoting.monthTaxRequest.Value + extraTax);

            Assert.AreEqual(extraTax, chaoting.extraTax);
            
        }

        [Test()]
        public void Test_ChaotingOweTax()
        {
            Assert.AreEqual(0, chaoting.oweTax);

            var oweTax = 100.0M;
            chaoting.ReportMonthTax(chaoting.monthTaxRequest.Value - oweTax);

            Assert.AreEqual(oweTax, chaoting.oweTax);

            var plusTax = 120.0M;
            chaoting.ReportTaxPlus(plusTax);

            Assert.AreEqual(0, chaoting.oweTax);
            Assert.AreEqual(plusTax - oweTax, chaoting.extraTax);
        }
    }
}
