using DataVisit;
using GMData;
using Newtonsoft.Json;
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
        private GMData.Run.Date date;

        [SetUp]
        public void Init()
        {
            date = new GMData.Run.Date(GMRoot.initData.start_date);
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
                        Assert.AreEqual(d, date.day);
                        Assert.AreEqual(m, date.month);
                        Assert.AreEqual(y, date.year);

                        Assert.AreEqual(d + (m - 1) * 30 + (y - 1) * 360, date.total_days);
                        Assert.AreEqual($"{y}-{m}-{d}", date.desc);

                        date.Inc();
                    }
                }
            }
        }

        [Test()]
        public void TestCompare()
        {
            date.year = 2;
            date.month = 2;
            date.day = 2;

            Assert.True(date == (2, null, null));
            Assert.True(date == (null, 2, null));
            Assert.True(date == (null, null, 2));
            Assert.True(date == (2, 2, 2));
            Assert.True(date == (null, 2, 2));
            Assert.True(date == (2, 2, null));
            Assert.True(date == (2, null, 2));
            Assert.True(date == (null, 2, 2));

            Assert.True(date < (3, null, null));
            Assert.True(date < (3, null, 1));
            Assert.True(date < (3, 1, 1));
            Assert.True(date < (null, 2, 3));
            Assert.True(date < (null, 3, 1));

            Assert.True(date > (1, null, null));
            Assert.True(date > (1, 12, null));
            Assert.True(date > (null, null, 1));
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(date);
            date = JsonConvert.DeserializeObject<GMData.Run.Date>(json);

            TestInc();
        }
    }
}
