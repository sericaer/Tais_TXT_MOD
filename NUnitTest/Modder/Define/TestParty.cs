using DataVisit;
using GMData;
using GMData.Mod;
using NUnit.Framework;
using NUnitTest.Modder;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Event
{
    [TestFixture()]
    public class TestParty : TestModBase
    {
        private (string file, string content) TEST_PARTY_1 = ("TEST_PARTY_1.txt",
            @"
                relation =
                {
                    peer = TEST_PARTY_2
                    value = 20
                }
                relation =
                {
                    peer = TEST_PARTY_1
                    value = 100
                }
            ");

        private (string file, string content) TEST_PARTY_2 = ("TEST_PARTY_2.txt",
            @"
                relation =
                {
                    peer = TEST_PARTY_1
                    value = -20
                }
                relation =
                {
                    peer = TEST_PARTY_2
                    value = -100
                }
            ");


        [Test()]
        public void TestEventNotTrigger()
        {
            LoadModScript("party/", TEST_PARTY_1, TEST_PARTY_2);

            var party1 = GMRoot.define.parties.SingleOrDefault(x => x.key == TEST_PARTY_1.file.Replace(".txt", ""));
            Assert.NotNull(party1);
            Assert.AreEqual(2, party1.relation.Count);
            Assert.AreEqual(20, party1.relation.Single(x=>x.peer == "TEST_PARTY_2").value);
            Assert.AreEqual(100, party1.relation.Single(x => x.peer == "TEST_PARTY_1").value);

            var party2 = GMRoot.define.parties.SingleOrDefault(x => x.key == TEST_PARTY_2.file.Replace(".txt", ""));
            Assert.NotNull(party2);
            Assert.AreEqual(2, party2.relation.Count);
            Assert.AreEqual(-100, party2.relation.Single(x => x.peer == "TEST_PARTY_2").value);
            Assert.AreEqual(-20, party2.relation.Single(x => x.peer == "TEST_PARTY_1").value);
        }
    }
}
