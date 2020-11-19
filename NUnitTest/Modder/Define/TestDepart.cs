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
    public class TestDepart : TestModBase
    {
        private (string file, string content) TEST_DEPART_1 = ("TEST_DEPART_1.txt",
            @"
                color = {r=1, g=2, b=3}

                pop = {type = POP1, num = 1000, party = TEST1}
                pop = {type = POP2, num = 2000}
                pop = {type = POP3, num = 3000}
            ");

        private (string file, string content) TEST_DEPART_2 = ("TEST_DEPART_2.txt",
            @"
                color = {r=4, g=5, b=6}

                pop = {type = POP1, num = 4000}
                pop = {type = POP2, num = 5000, party = TEST2}
            ");

        [SetUp]
        public void Setup()
        {
            Visitor.SetVisitData(Demon.Init());
            ModFileSystem.Clear();
        }

        [Test()]
        public void TestDepartLoad()
        {
            LoadModScript("depart/", TEST_DEPART_1, TEST_DEPART_2);

            var depart1 = GMRoot.define.departs.SingleOrDefault(x => x.key == TEST_DEPART_1.file.Replace(".txt", ""));

            Assert.NotNull(depart1);
            Assert.AreEqual(3, depart1.pop_init.Count);

            Assert.AreEqual("POP1", depart1.pop_init[0].type);
            Assert.AreEqual(1000, depart1.pop_init[0].num);
            Assert.AreEqual("TEST1", depart1.pop_init[0].party);

            Assert.AreEqual("POP2", depart1.pop_init[1].type);
            Assert.AreEqual(2000, depart1.pop_init[1].num);
            Assert.AreEqual(null, depart1.pop_init[1].party);

            Assert.AreEqual("POP3", depart1.pop_init[2].type);
            Assert.AreEqual(3000, depart1.pop_init[2].num);
            Assert.AreEqual(null, depart1.pop_init[2].party);

            var depart2 = GMRoot.define.departs.SingleOrDefault(x => x.key == TEST_DEPART_2.file.Replace(".txt", ""));
            Assert.NotNull(depart2);

            Assert.AreEqual(2, depart2.pop_init.Count);

            Assert.AreEqual("POP1", depart2.pop_init[0].type);
            Assert.AreEqual(4000, depart2.pop_init[0].num);
            Assert.AreEqual(null, depart2.pop_init[0].party);

            Assert.AreEqual("POP2", depart2.pop_init[1].type);
            Assert.AreEqual(5000, depart2.pop_init[1].num);
            Assert.AreEqual("TEST2", depart2.pop_init[1].party);
        }
    }
}
