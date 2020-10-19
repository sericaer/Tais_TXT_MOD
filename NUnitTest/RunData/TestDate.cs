using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [SetUpFixture]
    public class TestsSetupClass
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            //Parser.Semantic.Visitor.SetValueFunc = Visitor.Set;
            //Parser.Semantic.Visitor.GetValueFunc = Visitor.Get;
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {

        }
    }

    [TestFixture()]
    public class TestDate : TestRunDataBase
    {
        [SetUp]
        public void Init()
        {
            GMRoot.runner = GMData.Run.Runner.Generate();
        }

        [Test()]
        public void TestInc()
        {
            for (int y = 1; y <= 10; y++)
            {
                for (int m = 1; m <= 12; m++)
                {
                    for (int d = 1; d <= 30; d++)
                    {
                        Assert.AreEqual(d, Visitor.Get("date.day"));
                        Assert.AreEqual(m, Visitor.Get("date.month"));
                        Assert.AreEqual(y, Visitor.Get("date.year"));

                        GMRoot.runner.date.Inc();
                    }
                }
            }
        }

        [Test()]
        public void TestCompare()
        {
            GMRoot.runner.date.year.Value = 2;
            GMRoot.runner.date.month.Value = 2;
            GMRoot.runner.date.day.Value = 2;

            Assert.True(GMRoot.runner.date == (2, null, null));
            Assert.True(GMRoot.runner.date == (null, 2, null));
            Assert.True(GMRoot.runner.date == (null, null, 2));
            Assert.True(GMRoot.runner.date == (2, 2, 2));
            Assert.True(GMRoot.runner.date == (null, 2, 2));
            Assert.True(GMRoot.runner.date == (2, 2, null));
            Assert.True(GMRoot.runner.date == (2, null, 2));
            Assert.True(GMRoot.runner.date == (null, 2, 2));

            Assert.True(GMRoot.runner.date < (3, null, null));
            Assert.True(GMRoot.runner.date < (3, null, 1));
            Assert.True(GMRoot.runner.date < (3, 1, 1));
            Assert.True(GMRoot.runner.date < (null, 2, 3));
            Assert.True(GMRoot.runner.date < (null, 3, 1));

            Assert.True(GMRoot.runner.date > (1, null, null));
            Assert.True(GMRoot.runner.date > (1, 12, null));
            Assert.True(GMRoot.runner.date > (null, null, 1));
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = GMRoot.runner.Serialize();
            GMRoot.runner = GMData.Run.Runner.Deserialize(json);

            TestInc();
        }
    }
}
