//using DataVisit;
//using Modder;
//using NUnit.Framework;
//using RunData;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//namespace UnitTest.RunData
//{
//    [TestFixture()]
//    public class TestProcess : TestRunData
//    {
//        [Test()]
//        public void Test_CollectPopTax_DayInc()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);


//            COLLECT_POP_TAX.Start();
//            Assert.AreEqual(Chaoting.inst.reportPopNum.Value * 0.001, COLLECT_POP_TAX.inst.expectTax);

//            int selected = 1;

//            COLLECT_POP_TAX.inst.SetLevel(selected);
//            for (int i = 0; i < 30; i++)
//            {
//                Assert.IsFalse(COLLECT_POP_TAX.isFinishedDay());
//                Assert.AreEqual(1, Root.inst.processes.Count);

//                Date.Inc();
//                Process.DaysInc();
//            }

//            Assert.IsTrue(COLLECT_POP_TAX.isFinishedDay());

//            var currPopTax = Root.def.pop_tax.Single(x => x.name == $"level{selected}");
//            var expectCollectedTax = Pop.all.Where(x => x.def.is_collect_tax).Sum(x => x.num.Value) * currPopTax.per_tax;
//            Assert.AreEqual(expectCollectedTax, COLLECT_POP_TAX.inst.collectedTax);

//            Pop.all.ForEach(pop =>
//            {
//                if (pop.def.consume != null)
//                {
//                    Assert.AreEqual(pop.def.consume.Value + currPopTax.consume_effect, pop.consume.Value);
//                }
//            });

//            Date.Inc();
//            Process.DaysInc();
//            Assert.AreEqual(0, Root.inst.processes.Count);
//        }

//        [Test()]
//        public void Test_CollectPopTax_Serialize()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);


//            COLLECT_POP_TAX.Start();

//            int selected = 1;

//            COLLECT_POP_TAX.inst.SetLevel(selected);
//            for (int i = 0; i < 30; i++)
//            {
//                Assert.IsFalse(COLLECT_POP_TAX.isFinishedDay());
//                Assert.AreEqual(1, Root.inst.processes.Count);

//                Date.Inc();
//                Process.DaysInc();
//            }

//            var json = Root.Serialize();
//            Root.Deserialize(json);

//            Assert.AreEqual(1, COLLECT_POP_TAX.inst.selectedLevel);
//            Assert.AreEqual(100, COLLECT_POP_TAX.inst.percent.Value);

//            var currPopTax = Root.def.pop_tax.Single(x => x.name == $"level{selected}");
//            var expectCollectedTax = Pop.all.Where(x => x.def.is_collect_tax).Sum(x => x.num.Value) * currPopTax.per_tax;

//            Assert.AreEqual(expectCollectedTax, COLLECT_POP_TAX.inst.collectedTax);

//            Pop.all.ForEach(pop =>
//            {
//                if (pop.def.consume != null)
//                {
//                    Assert.AreEqual(pop.def.consume.Value + currPopTax.consume_effect, pop.consume.value);
//                }
//            });

//            Date.Inc();
//            Process.DaysInc();
//            Assert.AreEqual(0, Root.inst.processes.Count);

//        }
//    }
//}
