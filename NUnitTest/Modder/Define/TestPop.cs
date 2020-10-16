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
    public class TestPop
    {
        private ModFileSystem modFileSystem;
        private GMData.Mod.Modder modder;

        private (string file, string content) TEST_POP_1 = ("TEST_POP1.txt",
            @"
                is_collect_tax = true
            ");

        private (string file, string content) TEST_POP_2 = ("TEST_POP2.txt",
            @"
                is_collect_tax = false
                consume = 100
            ");

        public TestPop()
        {
            ModFileSystem.Clear();

            modFileSystem = ModFileSystem.Generate(nameof(TestParty));
        }

        private void LoadDepart(params (string file, string content)[] events)
        {
            foreach (var fevent in events)
            {
                modFileSystem.AddFile("pop/", fevent.file, fevent.content);
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
        public void TestLoad()
        {
            LoadDepart(TEST_POP_1, TEST_POP_2);

            var pop1 = GMRoot.define.pops.SingleOrDefault(x => x.key == TEST_POP_1.file.Replace(".txt", ""));
            Assert.NotNull(pop1);

            Assert.AreEqual("TEST_POP1", pop1.key);
            Assert.True(pop1.is_collect_tax);
            Assert.Null(pop1.consume);

            var pop2 = GMRoot.define.pops.SingleOrDefault(x => x.key == TEST_POP_2.file.Replace(".txt", ""));
            Assert.NotNull(pop2);

            Assert.AreEqual("TEST_POP2", pop2.key);
            Assert.False(pop2.is_collect_tax);
            Assert.NotNull(pop2.consume);
            Assert.AreEqual(100, pop2.consume);
        }
    }
}
