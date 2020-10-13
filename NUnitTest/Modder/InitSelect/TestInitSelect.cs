//using Modder;
//using NUnit.Framework;
//using System;
//using System.Linq;
//using UnitTest.Modder.Mock;

//namespace UnitTest.Modder.Event
//{
//    [TestFixture()]
//    public class TestInitSelect
//    {
//        private ModFileSystem modFileSystem;

//        private (string file, string content) INIT_SELECT_TEST =("INIT_SELECT_TEST.txt",
//            @"
//            is_first = true

//            option =
//            {
//                select =
//                {
//                    assign = {item1.data1, 1}
//                }

//                next = INIT_SELECT_TEST_1
//            }
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data1, 2}
//                }

//                next = INIT_SELECT_TEST_2
//            }
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data1, 3}
//                }
//            }");

//        private (string file, string content) INIT_SELECT_TEST_1 = ("INIT_SELECT_TEST_1.txt",
//            @"
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data2, 11}
//                }

//                next = INIT_SELECT_TEST_1_1
//            }
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data2, 12}
//                }
//            }");

//        private (string file, string content) INIT_SELECT_TEST_2 = ("INIT_SELECT_TEST_2.txt",
//            @"
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data2, 21}
//                }
//            }
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data2, 22}
//                }
//            }");

//        private (string file, string content) INIT_SELECT_TEST_1_1 = ("INIT_SELECT_TEST_1_1.txt",
//            @"
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data3, 111}
//                }
//            }
//            option =
//            {
//                select =
//                {
//                    assign = {item1.data3, 112}
//                }
//            }");

//        public TestInitSelect()
//        {
//            ModDataVisit.InitVisitMap(typeof(Demon));
            
//            ModFileSystem.Clear();

//            modFileSystem = ModFileSystem.Generate(nameof(TestInitSelect));
//        }

//        private void LoadInitSelect(params (string file, string content)[] events)
//        {
//            foreach (var fevent in events)
//            {
//                modFileSystem.AddInitSelect(fevent.file, fevent.content);
//            }

//            Mod.Load(ModFileSystem.path);
//        }

//        [SetUp]
//        public void Setup()
//        {
//            ModDataVisit.InitVisitData(Demon.Init());
//            ModFileSystem.Clear();
//        }

//        [Test()]
//        public void TestEventNotTrigger()
//        {
//            LoadInitSelect(INIT_SELECT_TEST, INIT_SELECT_TEST_1, INIT_SELECT_TEST_2, INIT_SELECT_TEST_1_1);

//            var initSelect = InitSelect.Enumerate().Single(x => x.initSelect.isFirst).initSelect;

//            Assert.AreEqual("INIT_SELECT_TEST_DESC", initSelect.desc.Format);

//            Assert.AreEqual(3, initSelect.options.Length);
//            Assert.AreEqual("INIT_SELECT_TEST_OPTION_1_DESC", initSelect.options[0].desc.Format);
//            Assert.AreEqual("INIT_SELECT_TEST_OPTION_2_DESC", initSelect.options[1].desc.Format);
//            Assert.AreEqual("INIT_SELECT_TEST_OPTION_3_DESC", initSelect.options[2].desc.Format);

//            initSelect.options[0].Selected();
//            Assert.AreEqual(1, Demon.inst.item1.data1);

//            Assert.AreEqual("INIT_SELECT_TEST_1", initSelect.options[0].Next);

//            initSelect.options[1].Selected();
//            Assert.AreEqual(2, Demon.inst.item1.data1);
//            Assert.AreEqual("INIT_SELECT_TEST_2", initSelect.options[1].Next);

//            initSelect.options[2].Selected();
//            Assert.AreEqual(3, Demon.inst.item1.data1);
//            Assert.AreEqual("", initSelect.options[2].Next);
//        }
//    }
//}
