//using Modder;
//using NUnit.Framework;
//using System;
//using System.Linq;
//using UnitTest.Modder.Mock;

//namespace UnitTest.Modder.Event
//{
//    [TestFixture()]
//    public class TestEventDepart
//    {
//        public TestEventDepart()
//        {
//            ModDataVisit.InitVisitMap(typeof(Demon));

//            ModFileSystem.Clear();

//            var modFileSystem = ModFileSystem.Generate(nameof(TestEventDepart));
//            modFileSystem.AddDepartEvent("EVENT_TEST.txt",
//@"title = EVENT_DIFF_TITLE
//desc = EVENT_DIFF_DESC

//trigger =
//{
//	equal = {depart.data1, 11}
//}

//date = every_day

//occur = 1

//option =
//{
//    desc = EVENT_TEST_OPTION_1_DESC
//    select =
//    {
//        assign = {depart.data2, 101}
//    }
//}

//option =
//{
//    desc = EVENT_TEST_OPTION_2_DESC
//    select =
//    {
//        assign = {depart.data2, 102}
//    }
//}

//option =
//{
//    desc = EVENT_TEST_OPTION_3_DESC
//    select =
//    {
//        assign = {depart.data2, 103}
//    }
//}");

//            Mod.Load(ModFileSystem.path);
//        }

//        [SetUp]
//        public void Setup()
//        {
//            ModDataVisit.InitVisitData(Demon.Init());
//        }

//        [Test()]
//        public void TestEventNotTrigger()
//        {
//            foreach(var eventobj in Mod.EventProcess((1,1,1)))
//            {
//                Assert.Fail();
//            }
//        }

//        [Test()]
//        public void TestEventBase()
//        {

//            Demon.inst.departs[0].data1 = 11;
//            int eventCount = 0;
//            foreach(var eventobj in Mod.EventProcess((1, 1, 1)))
//            {
//                Assert.AreEqual(eventobj.title.Format, "EVENT_DIFF_TITLE");
//                Assert.AreEqual(eventobj.desc.Format, "EVENT_DIFF_DESC");

//                Assert.AreEqual(eventobj.options.Length, 3);
//                Assert.AreEqual(eventobj.options[0].desc.Format, "EVENT_TEST_OPTION_1_DESC");
//                Assert.AreEqual(eventobj.options[1].desc.Format, "EVENT_TEST_OPTION_2_DESC");
//                Assert.AreEqual(eventobj.options[2].desc.Format, "EVENT_TEST_OPTION_3_DESC");

//                eventobj.options[0].Selected();
//                Assert.AreEqual(Demon.inst.departs[0].data2, 101);

//                eventobj.options[1].Selected();
//                Assert.AreEqual(Demon.inst.departs[0].data2, 102);

//                eventobj.options[2].Selected();
//                Assert.AreEqual(Demon.inst.departs[0].data2, 103);

//                eventCount++;
//            }

//            Assert.AreEqual(eventCount, 1);
//        }

//        [Test()]
//        public void TestEventMulti()
//        {

//            Demon.inst.departs[0].data1 = 11;
//            Demon.inst.departs[1].data1 = 11;
//            int count = 0;
//            foreach (var eventobj in Mod.EventProcess((1, 1, 1)))
//            {
//                Assert.AreEqual(eventobj.title.Format, "EVENT_DIFF_TITLE");
//                Assert.AreEqual(eventobj.desc.Format, "EVENT_DIFF_DESC");

//                Assert.AreEqual(eventobj.options.Length, 3);
//                Assert.AreEqual(eventobj.options[0].desc.Format, "EVENT_TEST_OPTION_1_DESC");
//                Assert.AreEqual(eventobj.options[1].desc.Format, "EVENT_TEST_OPTION_2_DESC");
//                Assert.AreEqual(eventobj.options[2].desc.Format, "EVENT_TEST_OPTION_3_DESC");

//                eventobj.options[0].Selected();
//                Assert.AreEqual(Demon.inst.departs[count].data2, 101);

//                eventobj.options[1].Selected();
//                Assert.AreEqual(Demon.inst.departs[count].data2, 102);

//                eventobj.options[2].Selected();
//                Assert.AreEqual(Demon.inst.departs[count].data2, 103);

//                count++;
//            }

//            Assert.AreEqual(2, count);
//        }
//    }
//}
