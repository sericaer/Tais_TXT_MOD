using DataVisit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.DataVisit
{
    [TestFixture()]
    public class TestDataVisit
    {

        [Test()]
        public void TestVisitSetGetCurrData()
        {
            TestData.inst = new TestData()
            {
                sub = new TestDataSub { a = 14 },
                elems = new List<TestElem>()
                                             {
                                                 new TestElem(){ b= 20},
                                                 new TestElem(){ b= 21}
                                             }
            };

            Visitor.InitVisitMap(typeof(TestData));
            Visitor.SetVisitData(TestData.inst);

            Assert.AreEqual(Visitor.Get("sub.a"), 14);

            Visitor.Set("sub.a", 22);
            Assert.AreEqual(Visitor.Get("sub.a"), 22);

            Assert.AreEqual(Visitor.Get("sub.c"), 23);
            Assert.AreEqual(Visitor.Get("sub.elem.b"), 20);

            Visitor.SetCurr("elem", TestData.inst.elems[0]);
            Assert.AreEqual(Visitor.Get("elem.b"), TestData.inst.elems[0].b);

            Visitor.SetCurr("elem", TestData.inst.elems[1]);
            Assert.AreEqual(Visitor.Get("elem.b"), TestData.inst.elems[1].b);

            Visitor.Set("elem.b", 100);
            Assert.AreEqual(100, TestData.inst.elems[1].b);
            Assert.AreEqual(TestData.inst.elems[1].b, Visitor.Get("elem.b"));
        }

        [Test()]
        public void TestVisitSetGetData()
        {
            TestData.inst = new TestData()
            {
                sub = new TestDataSub { a = 14 },
                elems = new List<TestElem>()
                                             {
                                                 new TestElem(){ b= 20},
                                                 new TestElem(){ b= 21}
                                             }
            };

            Visitor.InitVisitMap(typeof(TestData));
            Visitor.SetVisitData(TestData.inst);

            Assert.AreEqual(Visitor.Get("sub.a"), 14);

            Visitor.Set("sub.a", 22);
            Assert.AreEqual(Visitor.Get("sub.a"), 22);

            Assert.AreEqual(Visitor.Get("sub.c"), 23);
            Assert.AreEqual(Visitor.Get("sub.elem.b"), 20);

            //int i = 0;
            //Visitor.Pos pos = null;
            //while (Visitor.EnumerateVisit("elem", ref pos))
            //{
            //    Assert.AreEqual(Visitor.Get("elem.b"), TestData.inst.elems[i].b);
            //    i++;
            //}
        }
    }

    public class TestData
    {
        public static TestData inst;

        [DataVisitorProperty("sub")]
        public TestDataSub sub;

        [DataVisitorPropertyArray("elem")]
        public List<TestElem> elems;
    }

    public class TestDataSub
    {
        [DataVisitorProperty("a")]
        public int a;

        [DataVisitorProperty("c")]
        public int c
        {
            get
            {
                return a + 1;
            }
        }

        [DataVisitorProperty("elem")]
        public TestElem elem
        {
            get
            {
                return TestData.inst.elems[0];
            }
        }
    }

    public class TestElem
    {
        [DataVisitorProperty("b")]
        public int b;
    }
}
