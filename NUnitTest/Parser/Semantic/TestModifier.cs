using System;
using DataVisit;
using NUnit.Framework;
using Parser.Semantic;
using Parser.Syntax;

namespace NUnitTest.ParserT.SemanticT
{
    [TestFixture()]
    public class TestModifier
    {
        public class TestDemon
        {
            [SemanticProperty("group")]
            public ModifierGroup group;
        }

        public TestModifier()
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
        public void TestBaseValue()
        {
            string raw = @"test_demon =
            {
                group =
                {
                    base = 100
                } 
            }";
            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.AreEqual(100, demo.group.CalcValue());
        }

        [Test()]
        public void TestModify()
        {
            string raw = @"test_demon =
            {
                group =
                {
                    base = 50

                    modifier =
                    {
                        value = 50
                        condition =
                        {
                            equal = {sub.a, 1}
                        }
                    }
                } 
            }";

            DataVisit.Visitor.InitVisitMap(typeof(TestData));
            DataVisit.Visitor.SetVisitData(TestData.inst);

            Parser.Semantic.Visitor.GetValueFunc = DataVisit.Visitor.Get;
            Parser.Semantic.Visitor.SetValueFunc = DataVisit.Visitor.Set;

            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.AreEqual(100, demo.group.CalcValue());

            TestData.inst.sub.a = 2;
            Assert.AreEqual(50, demo.group.CalcValue());
        }

        [Test()]
        public void TestModifyMulti()
        {
            string raw = @"test_demon =
            {
                group =
                {
                    base = 50

                    modifier =
                    {
                        value = 25
                        condition =
                        {
                            less = {sub.a, 10}
                        }
                    }

                    modifier =
                    {
                        value = 25
                        condition =
                        {
                            greater = {sub.a, 5}
                        }
                    }
                } 
            }";

            DataVisit.Visitor.InitVisitMap(typeof(TestData));
            DataVisit.Visitor.SetVisitData(TestData.inst);

            Parser.Semantic.Visitor.GetValueFunc = DataVisit.Visitor.Get;
            Parser.Semantic.Visitor.SetValueFunc = DataVisit.Visitor.Set;

            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            TestData.inst.sub.a = 6;
            Assert.AreEqual(100, demo.group.CalcValue());

            TestData.inst.sub.a = 11;
            Assert.AreEqual(75, demo.group.CalcValue());

            TestData.inst.sub.a = 1;
            Assert.AreEqual(75, demo.group.CalcValue());
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
            public int a;
        }
    }
}
