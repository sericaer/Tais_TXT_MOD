using DataVisit;
using GMData;
using GMData.Mod;
using NUnit.Framework;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Event
{
    [TestFixture()]
    public class TestParty
    {
        private ModFileSystem modFileSystem;
        private GMData.Mod.Modder modder;

        private (string file, string content) TEST_PARTY_1 = ("TEST_PARTY_1.txt",
            @"
            ");

        private (string file, string content) TEST_PARTY_2 = ("TEST_PARTY_2.txt",
            @"
            ");

        public TestParty()
        {
            Parser.Semantic.Visitor.SetValueFunc = Visitor.Set;
            Parser.Semantic.Visitor.GetValueFunc = Visitor.Get;

            ModFileSystem.Clear();

            modFileSystem = ModFileSystem.Generate(nameof(TestParty));
        }

        private void LoadParty(params (string file, string content)[] events)
        {
            foreach (var fevent in events)
            {
                modFileSystem.AddParty(fevent.file, fevent.content);
            }

            modder = new GMData.Mod.Modder(ModFileSystem.path);
        }

        [SetUp]
        public void Setup()
        {
            Visitor.SetVisitData(Demon.Init());
            ModFileSystem.Clear();
        }

        [Test()]
        public void TestEventNotTrigger()
        {
            LoadParty(TEST_PARTY_1, TEST_PARTY_2);

            var party1 = GMRoot.define.parties.SingleOrDefault(x => x.key == TEST_PARTY_1.file.Replace(".txt", ""));
            Assert.NotNull(party1);

            var party2 = GMRoot.define.parties.SingleOrDefault(x => x.key == TEST_PARTY_2.file.Replace(".txt", ""));
            Assert.NotNull(party2);
        }
    }
}
