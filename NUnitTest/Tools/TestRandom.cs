using System;
using System.Collections.Generic;
using NUnit.Framework;
using Parser.Semantic;
using Parser.Syntax;
using GMData.Tools;

namespace NUnitTest.Tools
{
    [TestFixture()]
    public class TestRandom
    {
        public TestRandom()
        {

        }

        [Test()]
        public void TestIsOccur()
        {
            Assert.True(GRandom.isOccur(100));
            Assert.False(GRandom.isOccur(0));

            double count = 0;
            int max = 100000;
            for(int i=0; i< max; i++)
            {
                if(GRandom.isOccur(10))
                {
                    count++;
                }
            }

            Assert.LessOrEqual(count/max, 0.11);
            Assert.GreaterOrEqual(count/max, 0.09);
        }

        [Test()]
        public void TestCalcGroup()
        {
            List<(string, double)> group = new List<(string, double)>()
            {
                ("TEST1", 70),
                ("TEST2", 20),
                ("TEST3", 10)
            };

            double Test1Count = 0;
            double Test2Count = 1;
            double Test3Count = 1;

            int max = 100000;
            for (int i = 0; i < max; i++)
            {
                var str = GRandom.CalcGroup(group);
                if (str == "TEST1")
                {
                    Test1Count++;
                }
                if (str == "TEST2")
                {
                    Test2Count++;
                }
                if (str == "TEST3")
                {
                    Test3Count++;
                }
            }

            Assert.LessOrEqual(Test1Count / max, 0.71);
            Assert.GreaterOrEqual(Test1Count / max, 0.69);

            Assert.LessOrEqual(Test2Count / max, 0.21);
            Assert.GreaterOrEqual(Test2Count / max, 0.19);

            Assert.LessOrEqual(Test3Count / max, 0.11);
            Assert.GreaterOrEqual(Test3Count / max, 0.09);
        }

        [Test()]
        public void TestCalcGroupWithEmpty()
        {
            List<(string, double)> group = new List<(string, double)>()
            {
                ("TEST1", 30),
                ("TEST2", 20),
            };

            double Test1Count = 0;
            double Test2Count = 1;

            int max = 100000;
            for (int i = 0; i < max; i++)
            {
                var str = GRandom.CalcGroup(group);
                if (str == "TEST1")
                {
                    Test1Count++;
                }
                if (str == "TEST2")
                {
                    Test2Count++;
                }
            }

            Assert.LessOrEqual(Test1Count / max, 0.31);
            Assert.GreaterOrEqual(Test1Count / max, 0.29);

            Assert.LessOrEqual(Test2Count / max, 0.21);
            Assert.GreaterOrEqual(Test2Count / max, 0.19);
        }

        [Test()]
        public void TestCalcGroupOverride()
        {
            List<(string, double)> group = new List<(string, double)>()
            {
                ("TEST1", 50),
                ("TEST2", 50),
                ("TEST3", 100)
            };

            double Test1Count = 0;
            double Test2Count = 1;
            double Test3Count = 1;

            int max = 100000;
            for (int i = 0; i < max; i++)
            {
                var str = GRandom.CalcGroup(group);
                if (str == "TEST1")
                {
                    Test1Count++;
                }
                if (str == "TEST2")
                {
                    Test2Count++;
                }
                if (str == "TEST3")
                {
                    Test3Count++;
                }
            }

            Assert.LessOrEqual(Test1Count / max, 0.26);
            Assert.GreaterOrEqual(Test1Count / max, 0.24);

            Assert.LessOrEqual(Test2Count / max, 0.26);
            Assert.GreaterOrEqual(Test2Count / max, 0.24);

            Assert.LessOrEqual(Test3Count / max, 0.51);
            Assert.GreaterOrEqual(Test3Count / max, 0.49);
        }
    }
}
