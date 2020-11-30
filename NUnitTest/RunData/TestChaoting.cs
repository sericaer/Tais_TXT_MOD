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
            Assert.AreEqual((int)(GMRoot.define.chaoting.reportPopPercent /100 * 123456), chaoting.reportPopNum);
            Assert.AreEqual(GMRoot.define.chaoting.tax_level, chaoting.requestTaxLevel);

            var levels = GMRoot.define.adjusts.Single(x => x.key == "REPORT_CHAOTING").levels;
            Assert.AreEqual(chaoting.CalcTax(GMRoot.define.chaoting.tax_level, chaoting.reportPopNum), chaoting.monthTaxRequest);
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

            chaoting.yearReportTax = 250;
            chaoting.yearRequestTax = 200;

            Assert.AreEqual(chaoting.yearReportTax - chaoting.yearRequestTax, chaoting.extraTax);
            Assert.AreEqual(0, chaoting.oweTax);

        }

        [Test()]
        public void Test_ChaotingOweTax()
        {
            Assert.AreEqual(0, chaoting.oweTax);

            chaoting.yearReportTax = 250;
            chaoting.yearRequestTax = 300;

            Assert.AreEqual(0, chaoting.extraTax);
            Assert.AreEqual(chaoting.yearRequestTax - chaoting.yearReportTax, chaoting.oweTax);
        }
    }
}
