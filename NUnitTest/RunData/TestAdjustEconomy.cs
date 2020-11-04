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
            foreach (var def in GMRoot.define.economy.incomes)
            {
                var adjust = adjustEconomy.incomeAdjusts.SingleOrDefault(x => x.def == def);
                Assert.NotNull(adjust);

                Assert.AreEqual(def.key, adjust.key);
                Assert.AreEqual(def.valid, adjust.valid.Value);
                Assert.AreEqual(def.init_level, adjust.level.Value);

                var level = def.levels[def.init_level-1];
                Assert.AreEqual(level.effect_pop_consume, adjust.effect_pop_consume.Value);
                Assert.AreEqual(level.percent, adjust.percent.Value);
            }

            foreach (var def in GMRoot.define.economy.outputs)
            {
                var adjust = adjustEconomy.outputAdjusts.SingleOrDefault(x => x.def == def);
                Assert.NotNull(adjust);

                Assert.AreEqual(def.key, adjust.key);
                Assert.AreEqual(def.valid, adjust.valid.Value);
                Assert.AreEqual(def.init_level, adjust.level.Value);

                var level = def.levels[def.init_level-1];
                Assert.AreEqual(level.percent, adjust.percent.Value);
            }
        }

        [Test()]
        public void Test_AdjustChanged()
        {
            foreach (var def in GMRoot.define.economy.incomes)
            {
                var adjust = adjustEconomy.incomeAdjusts.SingleOrDefault(x => x.def == def);
                Assert.NotNull(adjust);

                for(int i=1; i<=def.levels.Count; i++)
                {
                    adjust.level.Value = i;

                    var level = def.levels[i-1];
                    Assert.AreEqual(level.effect_pop_consume, adjust.effect_pop_consume.Value);
                    Assert.AreEqual(level.percent, adjust.percent.Value);
                }
            }

            foreach (var def in GMRoot.define.economy.outputs)
            {
                var adjust = adjustEconomy.outputAdjusts.SingleOrDefault(x => x.def == def);
                Assert.NotNull(adjust);

                for (int i = 1; i <= def.levels.Count; i++)
                {
                    adjust.level.Value = i;

                    var level = def.levels[i - 1];
                    Assert.AreEqual(level.percent, adjust.percent.Value);
                }
            }
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(adjustEconomy, Formatting.Indented);
            adjustEconomy = JsonConvert.DeserializeObject<GMData.Run.AdjustEconomy>(json);

            Test_Init();
        }
    }
}
