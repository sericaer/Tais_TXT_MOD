﻿using DataVisit;
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
            economy.incomeDetails.ForEach(x => x.Value.OnNext(100));
            economy.outputDetails.ForEach(x => x.Value.OnNext(200));
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.define.economy.curr, economy.curr.Value);

            Assert.AreEqual(GMRoot.define.economy.incomes.Count(), economy.incomeDetails.Count());
            Assert.AreEqual(economy.incomeDetails.Count() * 100, economy.incomeTotal.Value);

            Assert.AreEqual(GMRoot.define.economy.outputs.Count(), economy.outputDetails.Count());
            Assert.AreEqual(economy.outputDetails.Count() * 200, economy.outputTotal.Value);

            Assert.AreEqual(economy.incomeTotal.Value - economy.outputTotal.Value, economy.monthSurplus.Value);
        }

        [Test()]
        public void Test_EconomyDayInc()
        {
            var date = new GMData.Run.Date(GMRoot.initData.start_date);
            
            var curr = GMRoot.define.economy.curr;
            for (int i=1; i<=360*10; i++)
            {
                if (i % 30 == 0)
                {
                    curr += economy.monthSurplus.Value;
                }
                
                economy.DaysInc(date);
                date.Inc();

                Assert.AreEqual(curr, economy.curr.Value);
            }
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(economy, Formatting.Indented);
            economy = JsonConvert.DeserializeObject<GMData.Run.Economy>(json);

            economy.incomeDetails.ForEach(x => x.Value.OnNext(100));
            economy.outputDetails.ForEach(x => x.Value.OnNext(200));

            Test_Init();
        }
    }
}
