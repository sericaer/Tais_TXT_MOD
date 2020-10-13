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
//    public class TestInitData
//    {
//        [Test()]
//        public void TestGetSet()
//        {
//            var init = new InitData()
//            {
//                common = new InitData.Common()
//                {
//                    name = "TEST1",
//                    age = 34,
//                    party = "BACKGROUND1"
//                }
//            };

//            Visitor.InitVisitMap(typeof(InitData));
//            Visitor.SetVisitData(init);

//            Assert.AreEqual(init.common.name, Visitor.Get("init.name"));
//            Assert.AreEqual(init.common.age, Visitor.Get("init.age"));
//            Assert.AreEqual(init.common.party, Visitor.Get("init.party"));

//        }
//    }
//}
