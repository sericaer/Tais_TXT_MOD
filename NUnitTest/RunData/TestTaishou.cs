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
//    public class TestTaishou : TestRunData
//    {
//        [Test()]
//        public void Test_Init()
//        {
//            ModDataVisit.InitVisitMap(typeof(Root));

//            Root.Init(init);
//            ModDataVisit.InitVisitData(Root.inst);

//            Assert.AreEqual(init.common.name, Visitor.Get("taishou.name"));
//            Assert.AreEqual(init.common.age, Visitor.Get("taishou.age"));
//            Assert.AreEqual(init.common.party, Visitor.Get("taishou.party.type"));
//        }
//    }
//}
