//using Modder;
//using NUnit.Framework;
//using System;
//using System.Linq;
//using UnitTest.Modder.Mock;

//namespace UnitTest.Modder.Warn
//{
//    [TestFixture()]
//    public class TestWarnCommon
//    {
//        private ModFileSystem modFileSystem;

//        private (string file, string content) WARN_TEST_DATA_1 = ("WARN_TEST_DATA_1.txt",
//            @"
//            trigger =
//            {
//	            equal = {item1.data1, 11}
//            }
//            ");

//        private (string file, string content) WARN_TEST_DATA_2 = ("WARN_TEST_DATA_2.txt",
//            @"
//            trigger =
//            {
//	            equal = {item1.data2, 12}
//            }

//            desc = {WARN_TEST_DATA_2_NEW_DESC, item1.data2}");

//        public TestWarnCommon()
//        {
//            ModDataVisit.InitVisitMap(typeof(Demon));

//            ModFileSystem.Clear();

//            modFileSystem = ModFileSystem.Generate(nameof(TestWarnCommon));
//            modFileSystem.AddCommonWarn(WARN_TEST_DATA_2.file, WARN_TEST_DATA_2.content);
//        }

//        [SetUpFixture]
//        public class TestSetup
//        {
//            public TestSetup()
//            {

//            }
//        }

//        [SetUp]
//        public void Setup()
//        {
//            ModDataVisit.InitVisitData(Demon.Init());
//            ModFileSystem.Clear();
//        }

//        [Test()]
//        public void TestWarnNotTrigger()
//        {
//            LoadWarn(WARN_TEST_DATA_1, WARN_TEST_DATA_2);

//            Demon.inst.item1.data1 = 10;
//            Demon.inst.item1.data2 = 10;

//            var warns = Mod.WarnProcess().ToArray();

//            Assert.AreEqual(0, warns.Length);
//        }

//        [Test()]
//        public void TestWarnTriggerSecond()
//        {
//            LoadWarn(WARN_TEST_DATA_1, WARN_TEST_DATA_2);

//            Demon.inst.item1.data1 = 11;
//            Demon.inst.item1.data2 = 12;

//            var warns = Mod.WarnProcess().ToArray();

//            Assert.AreEqual(2, warns.Length);

//            var warn1 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DATA_1");
//            Assert.NotNull(warn1);
//            Assert.AreEqual(1, warn1.desc.Count);
//            Assert.AreEqual("WARN_TEST_DATA_1_DESC", warn1.desc[0].Format);
//            Assert.AreEqual(0, warn1.desc[0].Params.Length);

//            var warn2 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DATA_2");
//            Assert.NotNull(warn2);
//            Assert.AreEqual(1, warn2.desc.Count);
//            Assert.AreEqual("WARN_TEST_DATA_2_NEW_DESC", warn2.desc[0].Format);
//            Assert.AreEqual(1, warn2.desc[0].Params.Length);
//            Assert.AreEqual("12", warn2.desc[0].Params[0]);
//        }

//        private void LoadWarn(params (string file, string content)[] warns)
//        {
//            foreach (var warn in warns)
//            {
//                modFileSystem.AddCommonWarn(warn.file, warn.content);
//            }

//            Mod.Load(ModFileSystem.path);
//        }
//    }
//}
