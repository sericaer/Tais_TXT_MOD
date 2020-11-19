using DataVisit;
using GMData;
using GMData.Mod;
using NUnit.Framework;
using NUnitTest.Modder;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Event
{
    [TestFixture()]
    public class TestPop : TestModBase
    {
        private (string file, string content) TEST_POP_1 = ("TEST_POP1.txt",
            @"
                is_collect_tax = true
                is_family = false
            ");

        private (string file, string content) TEST_POP_2 = ("TEST_POP2.txt",
            @"
                is_collect_tax = false
                is_family = true
                consume = 100
            ");


        [Test()]
        public void TestLoad()
        {
            LoadModScript("pop/", TEST_POP_1, TEST_POP_2);

            var pop1 = GMRoot.define.pops.SingleOrDefault(x => x.key == TEST_POP_1.file.Replace(".txt", ""));
            Assert.NotNull(pop1);

            Assert.AreEqual("TEST_POP1", pop1.key);
            Assert.True(pop1.is_collect_tax);
            Assert.False(pop1.is_family);
            Assert.Null(pop1.consume);

            var pop2 = GMRoot.define.pops.SingleOrDefault(x => x.key == TEST_POP_2.file.Replace(".txt", ""));
            Assert.NotNull(pop2);

            Assert.AreEqual("TEST_POP2", pop2.key);
            Assert.False(pop2.is_collect_tax);
            Assert.True(pop2.is_family);
            Assert.NotNull(pop2.consume);
            Assert.AreEqual(100, pop2.consume);
        }
    }
}
