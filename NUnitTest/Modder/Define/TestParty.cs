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
            ");

        private (string file, string content) TEST_PARTY_2 = ("TEST_PARTY_2.txt",
            @"
            ");


        [Test()]
        public void TestEventNotTrigger()
        {
            LoadModScript("party/", TEST_PARTY_1, TEST_PARTY_2);

            var party1 = GMRoot.define.parties.SingleOrDefault(x => x.key == TEST_PARTY_1.file.Replace(".txt", ""));
            Assert.NotNull(party1);

            var party2 = GMRoot.define.parties.SingleOrDefault(x => x.key == TEST_PARTY_2.file.Replace(".txt", ""));
            Assert.NotNull(party2);
        }
    }
}
