using DataVisit;
using GMData;
using GMData.Mod;
using NUnit.Framework;
using NUnitTest.Modder;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Define
{
    [TestFixture()]
    public class TestCommon : TestModBase
    {
        private (string file, string content) TEST_COMMON = ("common_define.txt",
            @"
                economy =
                {
                    curr = 123,
                    income_percent_pop_tax = 10,
                    output_percent_admin = 20,
                    output_percent_chaoting_tax = 30
                }
            ");

        [SetUp]
        public void Setup()
        {
            Visitor.SetVisitData(Demon.Init());
            ModFileSystem.Clear();
        }

        [Test()]
        public void TestEconomy()
        {
            LoadModScript("common/", TEST_COMMON);

            Assert.AreEqual(123, GMRoot.define.economy.curr);
            //Assert.AreEqual(10, GMRoot.define.economy.income_percent_pop_tax);
            //Assert.AreEqual(20, GMRoot.define.economy.output_percent_admin);
            //Assert.AreEqual(30, GMRoot.define.economy.output_percent_chaoting_tax);
        }
    }
}
