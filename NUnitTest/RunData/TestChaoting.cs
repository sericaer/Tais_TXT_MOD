using DataVisit;
using GMData;
using GMData.Def;
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
        private GMData.Def.IChaoting def;

        [SetUp]
        public void Init()
        {
            def = new CHAOTING_DEF_MOCK()
            {
                reportPopPercent = 110,
                powerParty = "TEST_PARY",
                taxInfo = new TaxInfo()
                {
                    init_level = 3,
                    levels = new List<TAX_LEVEL>()
                    {
                        new TAX_LEVEL(){per_person = 0.001M},
                        new TAX_LEVEL(){per_person = 0.0015M},
                        new TAX_LEVEL(){per_person = 0.002M},
                        new TAX_LEVEL(){per_person = 0.003M},
                        new TAX_LEVEL(){per_person = 0.005M},
                        new TAX_LEVEL(){per_person = 0.007M},
                        new TAX_LEVEL(){per_person = 0.01M},
                    }
                }
            };

            GMData.Run.Chaoting.getDef = () => def;

            chaoting = new GMData.Run.Chaoting(123456);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(def.powerParty, chaoting.powerPartyName);
            Assert.AreEqual((int)(def.reportPopPercent /100 * 123456), chaoting.reportPopNum);
            Assert.AreEqual(def.taxInfo.init_level, chaoting.requestTaxLevel);

            var level_effect = def.taxInfo.levels[chaoting.requestTaxLevel - 1];
            Assert.AreEqual(level_effect.per_person * chaoting.reportPopNum, chaoting.monthTaxRequest);
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
