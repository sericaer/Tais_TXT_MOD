using DataVisit;
using GMData;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestTaishou : TestRunDataBase
    {
        private GMData.Run.Taishou taishou;

        [SetUp]
        public void Init()
        {
            taishou = new GMData.Run.Taishou(GMRoot.initData.taishou);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.initData.taishou.name, taishou.name);
            Assert.AreEqual(GMRoot.initData.taishou.age, taishou.age.Value);
            Assert.AreEqual(GMRoot.initData.taishou.party, taishou.partyName);
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(taishou);
            taishou = JsonConvert.DeserializeObject<GMData.Run.Taishou>(json);

            Test_Init();
        }
    }
}
