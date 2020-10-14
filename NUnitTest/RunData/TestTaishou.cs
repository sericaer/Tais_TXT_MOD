using DataVisit;
using GMData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestTaishou
    {
        [SetUp]
        public void Init()
        {
            GMRoot.initData = new GMData.Init.InitData();
            GMRoot.initData.taishou.name = "TEST_NAME";
            GMRoot.initData.taishou.age = 123;
            GMRoot.initData.taishou.party = "TEST_PARTY";

            GMRoot.runner = new GMData.Run.Runner();
            GMRoot.runner.partys.Add(new GMData.Run.Party(GMRoot.initData.taishou.party));
        }

        [Test()]
        public void Test_Init()
        {

            Assert.AreEqual(GMRoot.initData.taishou.name, Visitor.Get("taishou.name"));
            Assert.AreEqual(GMRoot.initData.taishou.age, Visitor.Get("taishou.age"));
            Assert.AreEqual(GMRoot.initData.taishou.party, Visitor.Get("taishou.party.type"));
        }
    }
}
