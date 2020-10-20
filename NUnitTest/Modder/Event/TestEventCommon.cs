using DataVisit;
using NUnit.Framework;
using NUnitTest.Modder;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Event
{
    [TestFixture()]
    public class TestEventCommon : TestModBase
    {
        private (string file, string content) EVENT_TEST = ("EVENT_TEST.txt",
            @"title = EVENT_DIFF_TITLE
            desc = EVENT_DIFF_DESC

            trigger =
            {
	            equal = {item1.data1, 11}
            }

            date = every_day

            occur = 1

            option =
            {
                desc = { EVENT_TEST_OPTION_1_DESC, 11}
                select =
                {
                    assign = {item1.data2, 101}
                }
            }

            option =
            {
                desc = EVENT_TEST_OPTION_2_DESC
                select =
                {
                    assign = {item1.data2, 102}
                }
            }

            option =
            {
                desc = EVENT_TEST_OPTION_3_DESC
                select =
                {
                    assign = {item1.data2, 103}
                }
            }
            option =
            {
                desc = EVENT_TEST_OPTION_4_DESC
                select =
                {
                    assign = {item1.data3, true}
                }
            }");

        private (string file, string content) EVENT_TEST_DEFAULT = ("EVENT_TEST_DEFAULT.txt",
            @"
            trigger =
            {
	            equal = {item1.data1, 12}
            }

            date = every_day

            occur = 1

            option =
            {
                select =
                {
                    add = {item1.data2, 0}
                }
            }

            option =
            {
                select =
                {
                    add = {item1.data2, 1}
                }
            }

            option =
            {
                select =
                {
                    add = {item1.data2, 2}
                }
            }");

        private (string file, string content) EVENT_TEST_DATE_ONLY = ("EVENT_TEST_DATE_ONLY.txt",
            @"

            trigger = true

            date = {day = 22}

            occur = 1

            option =
            {
                select =
                {
                    assign = {item1.data2, 101}
                }
            }

            option =
            {
                select =
                {
                    assign = {item1.data2, 102}
                }
            }

            option =
            {
                select =
                {
                    assign = {item1.data2, 103}
                }
            }");

        private (string file, string content) EVENT_TEST_DATE_WITH_TRIGGER = ("EVENT_TEST_DATE_WITH_TRIGGER.txt",
            @"

            date = {month = 4, day = 22}

            trigger =
            {
	            equal = {item1.data1, 55}
            }

            occur = 1

            option =
            {
                select =
                {
                    assign = {item1.data2, 101}
                }
            }

            option =
            {
                select =
                {
                    assign = {item1.data2, 102}
                }
            }

            option =
            {
                select =
                {
                    assign = {item1.data2, 103}
                }
            }");

        private (string file, string content) EVENT_TEST_CONDITION_LESS = ("EVENT_TEST_CONDITION_LESS.txt",
        @"

            date = every_day

            trigger =
            {
	            less = {item1.data1, 55}
            }

            occur = 1

            option =
            {
                select =
                {
                    assign = {item1.data2, 101}
                }
            }

            option =
            {
                select =
                {
                    assign = {item1.data2, 102}
                }
            }

            option =
            {
                select =
                {
                    assign = {item1.data2, 103}
                }
            }");

        private (string file, string content) EVENT_TEST_NEXT = ("EVENT_TEST_NEXT.txt",
        @"

            date = every_day

            trigger =
            {
	            equal = {1, 1}
            }

            occur = 1

            option =
            {

            }

            option =
            {
                next =
                {
                    EVENT_TEST_CONDITION_LESS =
                    {
                        equal = {item1.data1, 1}
                    }
                    EVENT_TEST_DATE_WITH_TRIGGER =
                    {
                        equal = {item1.data1, 2}
                    }
                    default = EVENT_TEST_NEXT_RANDOM
                }
            }");

        private (string file, string content) EVENT_TEST_NEXT_RANDOM = ("EVENT_TEST_NEXT_RANDOM.txt",
        @"

            date = every_day

            trigger =
            {
	            equal = {1, 1}
            }

            occur = 1

            option =
            {

            }

            option =
            {
                next =
                {
                    EVENT_TEST_CONDITION_LESS =
                    {
                        equal = {item1.data1, 1}
                    }
                    EVENT_TEST_DATE_WITH_TRIGGER =
                    {
                        equal = {item1.data1, 2}
                    }
                }

                next_random = 
                {
                    EVENT_TEST_DATE_WITH_TRIGGER = 
                    {
                        modifier = 
                        {
                            value = 100
                        }
                    }
                }
            }");

        public TestEventCommon() : base()
        {
            Parser.Semantic.Visitor.SetValueFunc = Visitor.Set;
            Parser.Semantic.Visitor.GetValueFunc = Visitor.Get;

            Visitor.InitVisitMap(typeof(Demon));
        }

        [SetUp]
        public void SetUpCase()
        {
            Visitor.SetVisitData(Demon.Init());
        }

        [Test()]
        public void TestEventNotTrigger()
        {
            LoadModScript("/event/common/", EVENT_TEST, EVENT_TEST_DEFAULT);

            Demon.inst.item1.data1 = 10;

            Assert.AreEqual(0, modder.events.Count());
        }

        [Test()]
        public void TestEventBase()
        {
            LoadModScript("/event/common/", EVENT_TEST, EVENT_TEST_DEFAULT);

            Demon.inst.item1.data1 = 11;

            var eventobjs = modder.events.ToArray();

            Assert.AreEqual(1, eventobjs.Count());

            var eventobj = eventobjs[0];

            Assert.AreEqual("EVENT_DIFF_TITLE", eventobj.title.Format);
            Assert.AreEqual("EVENT_DIFF_DESC", eventobj.desc.Format);

            Assert.AreEqual(4, eventobj.options.Length);
            Assert.AreEqual("EVENT_TEST_OPTION_1_DESC", eventobj.options[0].desc.Format);
            Assert.AreEqual(1, eventobj.options[0].desc.Params.Count());
            Assert.AreEqual("11", eventobj.options[0].desc.Params[0]);

            Assert.AreEqual("EVENT_TEST_OPTION_2_DESC", eventobj.options[1].desc.Format);
            Assert.AreEqual("EVENT_TEST_OPTION_3_DESC", eventobj.options[2].desc.Format);

            eventobj.options[0].Selected();
            Assert.AreEqual(101, Demon.inst.item1.data2);

            eventobj.options[1].Selected();
            Assert.AreEqual(102, Demon.inst.item1.data2);

            eventobj.options[2].Selected();
            Assert.AreEqual(103, Demon.inst.item1.data2);

            eventobj.options[3].Selected();
            Assert.AreEqual(true, Demon.inst.item1.data3);

        }

        [Test()]
        public void TestEventCommonDefaultAndAdd()
        {

            LoadModScript("/event/common/", EVENT_TEST, EVENT_TEST_DEFAULT);

            Demon.inst.item1.data1 = 12;
            Demon.inst.item1.data2 = 101;

            var eventobjs = modder.events.ToArray();

            Assert.AreEqual(1, eventobjs.Count());

            var eventobj = eventobjs[0];

            Assert.AreEqual("EVENT_TEST_DEFAULT_TITLE", eventobj.title.Format);
            Assert.AreEqual("EVENT_TEST_DEFAULT_DESC", eventobj.desc.Format);

            Assert.AreEqual(eventobj.options.Length, 3);
            Assert.AreEqual("EVENT_TEST_DEFAULT_OPTION_1_DESC", eventobj.options[0].desc.Format);
            Assert.AreEqual("EVENT_TEST_DEFAULT_OPTION_2_DESC", eventobj.options[1].desc.Format);
            Assert.AreEqual("EVENT_TEST_DEFAULT_OPTION_3_DESC", eventobj.options[2].desc.Format);

            eventobj.options[0].Selected();
            Assert.AreEqual(101, Demon.inst.item1.data2);

            eventobj.options[1].Selected();
            Assert.AreEqual(102, Demon.inst.item1.data2);

            eventobj.options[2].Selected();
            Assert.AreEqual(104, Demon.inst.item1.data2);

        }

        [Test()]
        public void TestEventDateOnly()
        {
            LoadModScript("/event/common/", EVENT_TEST_DATE_ONLY);

            var eventobjs = modder.events.ToArray();
            Assert.AreEqual(0, eventobjs.Count());

            Demon.inst.date.day.Value = 22;

            eventobjs = modder.events.ToArray();
            Assert.AreEqual(1, eventobjs.Count());
            Assert.AreEqual("EVENT_TEST_DATE_ONLY_TITLE", eventobjs[0].title.Format);
        }

        [Test()]
        public void TestEventDateWithTrigger()
        {
            LoadModScript("/event/common/", EVENT_TEST_DATE_WITH_TRIGGER);

            Demon.inst.item1.data1 = 55;

            var eventobjs = modder.events.ToArray();
            Assert.AreEqual(0, eventobjs.Count());

            Demon.inst.item1.data1 = 56;

            eventobjs = modder.events.ToArray();
            Assert.AreEqual(0, eventobjs.Count());


            Demon.inst.item1.data1 = 55;
            Demon.inst.date.month.Value = 4;
            Demon.inst.date.day.Value = 22;

            eventobjs = modder.events.ToArray();

            Assert.AreEqual(1, eventobjs.Count());
            Assert.AreEqual(eventobjs[0].title.Format, "EVENT_TEST_DATE_WITH_TRIGGER_TITLE");
        }

        [Test()]
        public void TestEventCommoLess()
        {
            LoadModScript("/event/common/", EVENT_TEST_CONDITION_LESS);

            Demon.inst.item1.data1 = 55;
            Demon.inst.item1.data2 = 101;

            var eventobjs = modder.events.ToArray();
            Assert.AreEqual(0, eventobjs.Count());

            Demon.inst.item1.data1 = 54;
            Demon.inst.item1.data2 = 101;

            eventobjs = modder.events.ToArray();

            Assert.AreEqual(1, eventobjs.Count());

            var eventobj = eventobjs[0];

            Assert.AreEqual("EVENT_TEST_CONDITION_LESS_TITLE", eventobj.title.Format);
            Assert.AreEqual("EVENT_TEST_CONDITION_LESS_DESC", eventobj.desc.Format);

            Assert.AreEqual(3, eventobj.options.Length);
            Assert.AreEqual("EVENT_TEST_CONDITION_LESS_OPTION_1_DESC", eventobj.options[0].desc.Format);
            Assert.AreEqual("EVENT_TEST_CONDITION_LESS_OPTION_2_DESC", eventobj.options[1].desc.Format);
            Assert.AreEqual("EVENT_TEST_CONDITION_LESS_OPTION_3_DESC", eventobj.options[2].desc.Format);

            eventobj.options[0].Selected();
            Assert.AreEqual(101, Demon.inst.item1.data2);

            eventobj.options[1].Selected();
            Assert.AreEqual(102, Demon.inst.item1.data2);

            eventobj.options[2].Selected();
            Assert.AreEqual(103, Demon.inst.item1.data2);

        }

        //[Test()]
        //public void TestEventOptionNext()
        //{
        //    LoadEvent(EVENT_TEST_NEXT);

        //    var eventobjs = Mod.EventProcess((1, 1, 1)).ToArray();

        //    Assert.AreEqual(1, eventobjs.Count());

        //    var eventobj = eventobjs[0];
        //    Assert.AreEqual("", eventobj.options[0].Next);

        //    Demon.inst.item1.data1 = 1;
        //    Assert.AreEqual("EVENT_TEST_CONDITION_LESS", eventobj.options[1].Next);

        //    Demon.inst.item1.data1 = 2;
        //    Assert.AreEqual("EVENT_TEST_DATE_WITH_TRIGGER", eventobj.options[1].Next);

        //    Demon.inst.item1.data1 = 10;
        //    Assert.AreEqual("EVENT_TEST_NEXT_RANDOM", eventobj.options[1].Next);
        //}

        //[Test()]
        //public void TestEventOptionNextRandom()
        //{
        //    LoadEvent(EVENT_TEST_NEXT_RANDOM);

        //    var eventobjs = Mod.EventProcess((1, 1, 1)).ToArray();

        //    Assert.AreEqual(1, eventobjs.Count());

        //    var eventobj = eventobjs[0];

        //    eventobj.options[0].Selected();
        //    Assert.AreEqual("", eventobj.options[0].Next);

        //    Demon.inst.item1.data1 = 1;
        //    Assert.AreEqual("EVENT_TEST_CONDITION_LESS", eventobj.options[1].Next);

        //    Demon.inst.item1.data1 = 2;
        //    Assert.AreEqual("EVENT_TEST_DATE_WITH_TRIGGER", eventobj.options[1].Next);
        //}

        //private void LoadEvent(params (string file, string content)[] events)
        //{
        //    foreach (var fevent in events)
        //    {
        //        modFileSystem.AddCommonEvent(fevent.file, fevent.content);
        //    }

        //    Mod.Load(ModFileSystem.path);
        //}
    }
}
