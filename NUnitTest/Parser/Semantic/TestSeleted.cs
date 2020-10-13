using System;
using DataVisit;
using NUnit.Framework;
using Parser.Semantic;
using Parser.Syntax;

namespace NUnitTest.ParserT.SemanticT
{
    [TestFixture()]
    public class TestSelected
    {
        public class TestDemon
        {
            [SemanticProperty("selected")]
            public Select operation;
        }

        public TestSelected()
        {
            TestData.inst = new TestData()
            {
                sub = new TestDataSub()
                {
                    a = 1
                }
            };
        }

        [Test()]
        public void TestAssgin()
        {
            string raw = @"test_demon =
            {
                selected =
                {
                    assign = {sub.a, 123}
                }
            }";

            DataVisit.Visitor.InitVisitMap(typeof(TestData));
            DataVisit.Visitor.SetVisitData(TestData.inst);

            Parser.Semantic.Visitor.GetValueFunc = DataVisit.Visitor.Get;
            Parser.Semantic.Visitor.SetValueFunc = DataVisit.Visitor.Set;

            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            demo.operation.Do();

            Assert.AreEqual(123, TestData.inst.sub.a);
        }

        [Test()]
        public void TestAdd()
        {
            string raw = @"test_demon =
            {
                selected =
                {
                    add = {sub.a, 9}
                }
            }";

            DataVisit.Visitor.InitVisitMap(typeof(TestData));
            DataVisit.Visitor.SetVisitData(TestData.inst);

            Parser.Semantic.Visitor.GetValueFunc = DataVisit.Visitor.Get;
            Parser.Semantic.Visitor.SetValueFunc = DataVisit.Visitor.Set;

            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            TestData.inst.sub.a = 1;

            demo.operation.Do();

            Assert.AreEqual(10, TestData.inst.sub.a);
        }

        [Test()]
        public void TestReduce()
        {
            string raw = @"test_demon =
            {
                selected =
                {
                    reduce = {sub.a, 1}
                }
            }";

            DataVisit.Visitor.InitVisitMap(typeof(TestData));
            DataVisit.Visitor.SetVisitData(TestData.inst);

            Parser.Semantic.Visitor.GetValueFunc = DataVisit.Visitor.Get;
            Parser.Semantic.Visitor.SetValueFunc = DataVisit.Visitor.Set;

            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            TestData.inst.sub.a = 1;

            demo.operation.Do();

            Assert.AreEqual(0, TestData.inst.sub.a);
        }
    }

    public class TestData
    {
        public static TestData inst;

        [DataVisitorProperty("sub")]
        public TestDataSub sub;
    }

    public class TestDataSub
    {
        [DataVisitorProperty("a")]
        public double a;
    }
}
