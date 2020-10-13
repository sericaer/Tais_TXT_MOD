using System;
using NUnit.Framework;
using Parser.Semantic;
using Parser.Syntax;

namespace NUnitTest.ParserT.SemanticT
{
    [TestFixture()]
    public class TestCondition
    {
        public class TestDemon
        {
            [SemanticProperty("condition")]
            public Condition condition;
        }

        public TestCondition()
        {
            
        }

        [Test()]
        public void TestEqual()
        {
            string raw = @"test_demon =
            {
                condition =
                {
                    equal = {1, 1}
                }
            }";
            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsTrue(demo.condition.Rslt());

            raw = @"test_demon =
            {
                condition =
                {
                    equal = {1, 12}
                }
            }";
            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());
        }


        [Test()]
        public void TestLess()
        {
            string raw = @"test_demon =
            {
                condition =
                {
                    less = {1, 12}
                }
            }";
            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsTrue(demo.condition.Rslt());

            raw = @"test_demon =
            {
                condition =
                {
                    less = {1, 1}
                }
            }";
            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());

            raw = @"test_demon =
            {
                condition =
                {
                    less = {12, 1}
                }
            }";
            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());
        }

        [Test()]
        public void TestGreater()
        {
            string raw = @"test_demon =
            {
                condition =
                {
                    greater = {12, 1}
                }
            }";
            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsTrue(demo.condition.Rslt());

            raw = @"test_demon =
            {
                condition =
                {
                    greater = {1, 1}
                }
            }";
            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());

            raw = @"test_demon =
            {
                condition =
                {
                    greater = {1, 12}
                }
            }";
            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());


        }

        [Test()]
        public void TestNot()
        {
            string raw = @"test_demon =
            {
                condition =
                {
                    not =
                    {
                        less = {12, 1}
                    }
                }
            }";
            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsTrue(demo.condition.Rslt());


            raw = @"test_demon =
            {
                condition =
                {
                    not =
                    {
                        less = {1, 12}
                    }
                }
            }";

            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());
        }

        [Test()]
        public void TestAnd()
        {
            string raw = @"test_demon =
            {
                condition =
                {
                    and =
                    {
                        less = {1, 12}
                        greater = {1, 0}
                    }
                }
            }";

            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsTrue(demo.condition.Rslt());


            raw = @"test_demon =
            {
                condition =
                {
                    and =
                    {
                        less = {1, 12}
                        greater = {1, 12}
                    }
                }
            }";

            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());

            raw = @"test_demon =
            {
                condition =
                {
                    and =
                    {
                        less = {1, 0}
                        greater = {1, 12}
                    }
                }
            }";

            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());
        }

        [Test()]
        public void TestOr()
        {
            string raw = @"test_demon =
            {
                condition =
                {
                    or =
                    {
                        less = {1, 12}
                        greater = {1, 0}
                    }
                }
            }";

            var syntaxItem = SyntaxItem.RootParse(raw);
            TestDemon demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsTrue(demo.condition.Rslt());


            raw = @"test_demon =
            {
                condition =
                {
                    or =
                    {
                        less = {1, 12}
                        greater = {1, 12}
                    }
                }
            }";

            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsTrue(demo.condition.Rslt());

            raw = @"test_demon =
            {
                condition =
                {
                    or =
                    {
                        less = {1, 0}
                        greater = {1, 12}
                    }
                }
            }";

            syntaxItem = SyntaxItem.RootParse(raw);
            demo = SemanticParser.DoParser<TestDemon>(syntaxItem.Find("test_demon"));

            Assert.IsFalse(demo.condition.Rslt());
        }
    }
}
