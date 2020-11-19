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
    public class TestAdjust : TestRunDataBase
    {
        private GMData.Run.Adjust adjust;

        [SetUp]
        public void Init()
        {
            adjust = new GMData.Run.Adjust(GMData.Run.Adjust.EType.POP_TAX);
        }

        [Test()]
        public void Test_Enum()
        {
            Assert.True(GMData.Run.Adjust.EType.POP_TAX.HasAttribute<GMData.Run.Adjust.EconomyInput>());

            Assert.True(GMData.Run.Adjust.EType.ADMIN_SPEND.HasAttribute<GMData.Run.Adjust.EconomyOutput>());
            Assert.True(GMData.Run.Adjust.EType.REPORT_CHAOTING.HasAttribute<GMData.Run.Adjust.EconomyOutput>());

        }

        [Test()]
        public void Test_Init()
        {
            var popTaxAdjust = GMRoot.define.adjusts.Single(x => x.key == GMData.Run.Adjust.EType.POP_TAX.ToString());
            Assert.AreEqual(popTaxAdjust.init.level, adjust.level);
            Assert.AreEqual(popTaxAdjust.init.valid, adjust.valid);
            Assert.AreEqual(popTaxAdjust, adjust.def);
            Assert.AreEqual(popTaxAdjust.levels[popTaxAdjust.init.level-1], adjust.levelDef);
        }

        [Test()]
        public void Test_Serialize()
        {
            adjust.level = 5;

            var json = JsonConvert.SerializeObject(adjust, Formatting.Indented);
            adjust = JsonConvert.DeserializeObject<GMData.Run.Adjust>(json);

            Assert.AreEqual(5, adjust.level);
        }
    }
}
