//using Modder;
//using NUnit.Framework;
//using System;
//using System.Linq;
//using UnitTest.Modder.Mock;

//namespace UnitTest.Modder.Event
//{
//    [TestFixture()]
//    public class TestRisk
//    {
//        private ModFileSystem modFileSystem;

//        private (string file, string content) RISK_TEST = ("RISK_TEST.txt",
//            @"
//            cost_days = 30

//            end_event = EVENT_RISK_TEST_END

//            random_event = 
//            {
//                EVENT_RISK_TEST_RANDON_1 =
//                {
//                    modifier = 
//                    {
//                        value = 0.01
//                    }
//                }

//                EVENT_RISK_TEST_RANDON_2 =
//                {
//                    modifier = 
//                    {
//                        value = 0.02
//                    }
//                }
//            }");

//        public TestRisk()
//        {
//            ModDataVisit.InitVisitMap(typeof(Demon));

//            ModFileSystem.Clear();

//            modFileSystem = ModFileSystem.Generate(nameof(TestInitSelect));
//        }

//        private void LoadRisk(params (string file, string content)[] events)
//        {
//            foreach (var fevent in events)
//            {
//                modFileSystem.AddRisk(fevent.file, fevent.content);
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
//            LoadRisk(RISK_TEST);

//            var risk = Mod.def.risks.Single(x => x.key == "RISK_TEST");

//            Assert.AreEqual(30, risk.cost_days);
//            Assert.AreEqual("EVENT_RISK_TEST_END", risk.endEvent);

//            var rd1 = risk.randomEvent.list.SingleOrDefault(x => x.name == "EVENT_RISK_TEST_RANDON_1");
//            Assert.NotNull(rd1);
//            Assert.AreEqual(0.01, rd1.modifier.CalcValue());

//            var rd2 = risk.randomEvent.list.SingleOrDefault(x => x.name == "EVENT_RISK_TEST_RANDON_2");
//            Assert.NotNull(rd2);
//            Assert.AreEqual(0.02, rd2.modifier.CalcValue());
//        }
//    }
//}
