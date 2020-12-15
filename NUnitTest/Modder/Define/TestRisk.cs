using GMData;
using NUnit.Framework;
using NUnitTest.Modder;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Risk
{
    [TestFixture()]
    public class TestRisk : TestModBase
    {

        private (string file, string content) RISK_TEST = ("RISK_TEST.txt",
            @"
            cost_days = 30

            end_event = EVENT_RISK_TEST_END

            choice =
            {
		        desc = RISK_TEST_OPTION_1_DESC
		
                select = 
                {
                    reduce = {economy.value, 10}
                }
    
                random_event = 
                {
		                EVENT_RISK_TEST_RANDON_1 =
		                {
		                    base = 0.5
		                }
                }
            }

            choice =
            {   
		        desc = RISK_TEST_OPTION_2_DESC

                random_event = 
                {
		                EVENT_RISK_TEST_RANDON_2 =
		                {
		                    base = 2
		                }
                }
            }
");


        [Test()]
        public void TestLoad()
        {
            LoadModScript("/risk/", RISK_TEST);

            var risk = GMRoot.define.risks.Single(x => x.key == "RISK_TEST");

            Assert.AreEqual(30, risk.cost_days);
            Assert.AreEqual("EVENT_RISK_TEST_END", risk.endEvent);

            Assert.AreEqual(2, risk.options.Count());

            var opt1 = risk.options[0];
            Assert.AreEqual("RISK_TEST_OPTION_1_DESC", opt1.desc.name);
            Assert.AreEqual(1, opt1.randomEvent.Calc().Count());

            var randomEvent1 = opt1.randomEvent;
            Assert.AreEqual("EVENT_RISK_TEST_RANDON_1", randomEvent1.Calc().First().name);

            var opt2 = risk.options[1];
            Assert.AreEqual("RISK_TEST_OPTION_2_DESC", opt2.desc.name);

            Assert.AreEqual(1, opt2.randomEvent.Calc().Count());

            var randomEvent2 = opt2.randomEvent;
            Assert.AreEqual("EVENT_RISK_TEST_RANDON_2", randomEvent2.Calc().First().name);

        }
    }
}
