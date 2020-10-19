using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestTaishou : TestRunDataBase
    {
        [SetUp]
        public void Init()
        {
            GMRoot.runner = GMData.Run.Runner.Generate();
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.initData.taishou.name, Visitor.Get("taishou.name"));
            Assert.AreEqual(GMRoot.initData.taishou.age, Visitor.Get("taishou.age"));
            Assert.AreEqual(GMRoot.initData.taishou.party, Visitor.Get("taishou.party.type"));
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = GMRoot.runner.Serialize();
            GMRoot.runner = GMData.Run.Runner.Deserialize(json);

            Test_Init();
        }
    }
}
