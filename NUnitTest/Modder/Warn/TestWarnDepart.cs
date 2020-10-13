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

//        private (string file, string content) WARN_TEST_DEPART_DATA_1 = ("WARN_TEST_DEPART_DATA_1.txt",
//            @"
//            trigger =
//            {
//	            equal = {depart.data1, 11}
//            }");

//        private (string file, string content) WARN_TEST_DEPART_DATA_2 = ("WARN_TEST_DEPART_DATA_2.txt",
//            @"
//            trigger =
//            {
//	            equal = {depart.data2, 12}
//            }
//            desc = {depart.data2, 12}");

//        public TestWarnCommon()
//        {
//            ModDataVisit.InitVisitMap(typeof(Demon));

//            ModFileSystem.Clear();

//            modFileSystem = ModFileSystem.Generate(nameof(TestWarnCommon));
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
//            LoadWarn(WARN_TEST_DEPART_DATA_1, WARN_TEST_DEPART_DATA_2);

//            Demon.inst.departs[0].data1 = 10;
//            Demon.inst.departs[0].data2 = 10;

//            Demon.inst.departs[1].data1 = 10;
//            Demon.inst.departs[1].data2 = 10;

//            var warns = Mod.WarnProcess().ToArray();

//            Assert.AreEqual(0, warns.Length);
//        }

//        [Test()]
//        public void TestWarnTriggerOneDepart()
//        {
//            LoadWarn(WARN_TEST_DEPART_DATA_1, WARN_TEST_DEPART_DATA_2);

//            Demon.inst.departs[0].name = "depart1";
//            Demon.inst.departs[0].data1 = 11;
//            Demon.inst.departs[0].data2 = 12;

//            Demon.inst.departs[1].name = "depart2";
//            Demon.inst.departs[1].data1 = 10;
//            Demon.inst.departs[1].data2 = 10;

//            var warns = Mod.WarnProcess();

//            Assert.AreEqual(2, warns.Length);

//            var warn1 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DEPART_DATA_1");
//            Assert.NotNull(warn1);
//            Assert.AreEqual(1, warn1.datas.Count);
//            Assert.AreEqual("depart1", warn1.datas[0]);

//            var warn2 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DEPART_DATA_2");
//            Assert.NotNull(warn2);
//            Assert.AreEqual(1, warn2.datas.Count);
//            Assert.AreEqual("depart1|12|12", warn2.datas[0]);
//        }

//        [Test()]
//        public void TestWarnTriggerSecondDepart()
//        {
//            LoadWarn(WARN_TEST_DEPART_DATA_1, WARN_TEST_DEPART_DATA_2);

//            Demon.inst.departs[0].name = "depart1";
//            Demon.inst.departs[0].data1 = 11;
//            Demon.inst.departs[0].data2 = 12;

//            Demon.inst.departs[1].name = "depart2";
//            Demon.inst.departs[1].data1 = 11;
//            Demon.inst.departs[1].data2 = 12;

//            var warns = Mod.WarnProcess();

//            Assert.AreEqual(2, warns.Length);

//            var warn1 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DEPART_DATA_1");
//            Assert.NotNull(warn1);
//            Assert.AreEqual(2, warn1.datas.Count);
//            Assert.IsTrue(warn1.datas.Contains("depart1"));
//            Assert.IsTrue(warn1.datas.Contains("depart2"));

//            var warn2 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DEPART_DATA_2");
//            Assert.NotNull(warn2);
//            Assert.AreEqual(2, warn2.datas.Count);
//            Assert.IsTrue(warn2.datas.Contains("depart1|12|12"));
//            Assert.IsTrue(warn2.datas.Contains("depart2|12|12"));
//        }

//        private void LoadWarn(params (string file, string content)[] warns)
//        {
//            foreach (var warn in warns)
//            {
//                modFileSystem.AddDepartWarn(warn.file, warn.content);
//            }

//            Mod.Load(ModFileSystem.path);
//        }
//    }
//}
