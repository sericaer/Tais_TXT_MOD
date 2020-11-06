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
    public class TestAdjustEconomy : TestRunDataBase
    {
        private GMData.Run.AdjustEconomy adjustEconomy;

        [SetUp]
        public void Init()
        {
            adjustEconomy = new GMData.Run.AdjustEconomy(GMRoot.define.economy);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.define.economy.adjust_pop_tax.init_level, adjustEconomy.popTaxLevel.Value);
        }

        [Test()]
        public void Test_Serialize()
        {
            adjustEconomy.popTaxLevel.Value = 5;

            var json = JsonConvert.SerializeObject(adjustEconomy, Formatting.Indented);
            adjustEconomy = JsonConvert.DeserializeObject<GMData.Run.AdjustEconomy>(json);

            Assert.AreEqual(5, adjustEconomy.popTaxLevel.Value);
        }
    }
}
