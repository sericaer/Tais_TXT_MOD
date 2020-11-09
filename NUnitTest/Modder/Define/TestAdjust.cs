using GMData;
using NUnit.Framework;
using NUnitTest.Modder;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Event
{
    [TestFixture()]
    public class TestAdjust : TestModBase
    {

        private (string file, string content) POP_TAX = ("POP_TAX.txt",
            @"
init = 
{
    valid = true
    level = 3
}

level = 
{
    percent = 0
}

level = 
{
    percent = 5,
    effect_pop_consume = -5,
}

level = 
{
    percent = 10,
    effect_pop_consume = -10,
}

level = 
{
    percent = 20,
    effect_pop_consume = -15,
}

level = 
{
    percent = 30,
    effect_pop_consume = -20,
}

level = 
{
    percent = 40,
    effect_pop_consume = -30,
}

level = 
{
    percent = 50,
    effect_pop_consume = -40,
}
");


        [Test()]
        public void TestLoad()
        {
            LoadModScript("/adjust/", POP_TAX);

            var adjust = GMRoot.define.adjusts.Single(x => x.key == "POP_TAX");

            Assert.AreEqual(3, adjust.init.level);
            Assert.AreEqual(true, adjust.init.valid);

            Assert.AreEqual(7, adjust.levels.Count);

            Assert.AreEqual(0, adjust.levels[0].percent);
            Assert.IsNull(adjust.levels[0].effect_pop_consume);

            Assert.AreEqual(5, adjust.levels[1].percent);
            Assert.AreEqual(-5, adjust.levels[1].effect_pop_consume);

            Assert.AreEqual(10, adjust.levels[2].percent);
            Assert.AreEqual(-10, adjust.levels[2].effect_pop_consume);

            Assert.AreEqual(20, adjust.levels[3].percent);
            Assert.AreEqual(-15, adjust.levels[3].effect_pop_consume);

            Assert.AreEqual(30, adjust.levels[4].percent);
            Assert.AreEqual(-20, adjust.levels[4].effect_pop_consume);

            Assert.AreEqual(40, adjust.levels[5].percent);
            Assert.AreEqual(-30, adjust.levels[5].effect_pop_consume);

            Assert.AreEqual(50, adjust.levels[6].percent);
            Assert.AreEqual(-40, adjust.levels[6].effect_pop_consume);
        }
    }
}
