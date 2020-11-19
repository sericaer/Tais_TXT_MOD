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
    public class TestEconomy : TestRunDataBase
    {
        private GMData.Run.Economy economy;

        [SetUp]
        public void Init()
        {
            economy = new GMData.Run.Economy(GMRoot.define.economy);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.define.economy.curr, economy.curr);

            economy.detail.popTax = 200;

            economy.detail.reportChaoting = 10;
            economy.detail.adminSpend = 20;

            Assert.AreEqual(economy.detail.popTax, economy.incomeTotal);
            Assert.AreEqual(economy.detail.reportChaoting + economy.detail.adminSpend, economy.outputTotal);

            Assert.AreEqual(economy.incomeTotal - economy.outputTotal, economy.monthSurplus);
        }

        [Test()]
        public void Test_EconomyDayInc()
        {
            var date = new GMData.Run.Date(GMRoot.initData.start_date);

            economy.detail.popTax = 200;

            economy.detail.reportChaoting = 10;
            economy.detail.adminSpend = 20;

            var curr = (decimal)GMRoot.define.economy.curr;
            for (int i=1; i<=360*10; i++)
            {
                if (i % 30 == 0)
                {
                    curr += economy.monthSurplus;
                }
                
                economy.DaysInc(date);
                date.Inc();

                Assert.AreEqual(curr, economy.curr);
            }
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(economy, Formatting.Indented);
            economy = JsonConvert.DeserializeObject<GMData.Run.Economy>(json);

            Test_Init();
        }
    }
}
