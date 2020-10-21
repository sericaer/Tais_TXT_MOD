using DataVisit;
using NUnit.Framework;
using NUnitTest.Modder;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Warn
{
    [TestFixture()]
    public class TestWarnCommon : TestModBase
    {
        private (string file, string content) WARN_TEST_DATA_1 = ("WARN_TEST_DATA_1.txt",
            @"
            trigger =
            {
	            equal = {item1.data1, 11}
            }
            ");

        private (string file, string content) WARN_TEST_DATA_2 = ("WARN_TEST_DATA_2.txt",
            @"
            trigger =
            {
	            equal = {item1.data2, 12}
            }

            desc = {WARN_TEST_DATA_2_NEW_DESC, item1.data2}");

        public TestWarnCommon()
        {
            Parser.Semantic.Visitor.SetValueFunc = Visitor.Set;
            Parser.Semantic.Visitor.GetValueFunc = Visitor.Get;

            Visitor.InitVisitMap(typeof(Demon));
        }


        [SetUp]
        public void Setup()
        {
            Visitor.SetVisitData(Demon.Init());
        }

        [Test()]
        public void TestWarnNotTrigger()
        {
            LoadModScript("/warn/common/", WARN_TEST_DATA_1, WARN_TEST_DATA_2);

            Demon.inst.item1.data1 = 10;
            Demon.inst.item1.data2 = 10;

            var warns = modder.warns.ToArray();

            Assert.AreEqual(0, warns.Length);
        }

        [Test()]
        public void TestWarnTriggerSecond()
        {
            LoadModScript("/warn/common/", WARN_TEST_DATA_1, WARN_TEST_DATA_2);

            Demon.inst.item1.data1 = 11;
            Demon.inst.item1.data2 = 12;

            var warns = modder.warns.ToArray();

            Assert.AreEqual(2, warns.Length);

            var warn1 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DATA_1");
            Assert.NotNull(warn1);
            Assert.AreEqual("WARN_TEST_DATA_1_DESC", warn1.desc.Format);
            Assert.AreEqual(0, warn1.desc.Params.Length);

            var warn2 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DATA_2");
            Assert.NotNull(warn2);
            Assert.AreEqual("WARN_TEST_DATA_2_NEW_DESC", warn2.desc.Format);
            Assert.AreEqual(1, warn2.desc.Params.Length);
            Assert.AreEqual("12", warn2.desc.Params[0]);
        }
    }
}
