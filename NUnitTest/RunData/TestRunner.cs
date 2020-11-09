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
    public class TestRunner : TestRunDataBase
    {
        [SetUp]
        public void Init()
        {
            GMRoot.runner = new GMData.Run.Runner(GMRoot.initData);
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.initData.start_date.day, Visitor.Get("date.day"));
            Assert.AreEqual(GMRoot.initData.start_date.month, Visitor.Get("date.month"));
            Assert.AreEqual(GMRoot.initData.start_date.year, Visitor.Get("date.year"));

            Assert.AreEqual(GMRoot.define.economy.curr, Visitor.Get("economy.value"));

            foreach (var tax in GMRoot.runner.pops.SelectNotNull(x => x.tax))
            {
                Assert.True(tax.buffers.Contains("POP_TAX"));

                var popTaxAdjustDef = GMRoot.define.adjusts.Single(x => x.key == GMData.Run.Adjust.EType.POP_TAX.ToString());
                var percent = popTaxAdjustDef.levels[popTaxAdjustDef.init.level - 1].percent;

                Assert.AreEqual(percent, (double)tax.buffers["POP_TAX"] * 100.0/ tax.baseValue.Value);
            }

            //foreach (var def in GMRoot.define.economy.incomes)
            //{
            //    var currLevel = def.levels[def.init_level - 1];

            //    foreach (var consume in GMRoot.runner.pops.SelectNotNull(x => x.consume))
            //    {
            //        if (currLevel.effect_pop_consume != null)
            //        {
            //            Assert.True(consume.buffers.Contains(def.key));
            //            Assert.AreEqual(currLevel.effect_pop_consume * consume.baseValue.Value * 0.01, consume.buffers[def.key]);
            //        }
            //    }
            //}

            //var in_def = GMRoot.define.economy.incomes.Single(x => x.key == "POP_TAX");
            //var in_level = in_def.levels[in_def.init_level - 1];

            //foreach (var tax in GMRoot.runner.pops.SelectNotNull(x => x.tax))
            //{
            //    Assert.True(tax.buffers.Contains("POP_TAX"));
            //    Assert.AreEqual(in_level.percent * tax.baseValue.Value * 0.01, tax.buffers["POP_TAX"]);
            //}

            //var incomePopTax = GMRoot.runner.economy.incomeDetails.Single(x => x.type == GMData.Run.IncomeDetail.TYPE.POP_TAX);
            //Assert.AreEqual(GMRoot.runner.pops.Sum(x => x.tax?.value.Value), incomePopTax.Value.Value);

            //var out_def = GMRoot.define.economy.outputs.Single(x => x.key == "ADMIN");
            //var out_level = out_def.levels[out_def.init_level - 1];

            //foreach (var admin in GMRoot.runner.pops.SelectNotNull(x => x.adminExpend))
            //{
            //    Assert.True(admin.buffers.Contains("ADMIN"));
            //    Assert.AreEqual(out_level.percent * admin.baseValue.Value * 0.01, admin.buffers["ADMIN"]);
            //}

            //var outputPopAdmin = GMRoot.runner.economy.outputDetails.Single(x => x.type == GMData.Run.OutputDetail.TYPE.ADMIN);
            //Assert.AreEqual(GMRoot.runner.pops.Sum(x => x.adminExpend?.value.Value), outputPopAdmin.Value.Value);

            //out_def = GMRoot.define.economy.outputs.Single(x => x.key == "CHAOTING");
            //var outputChaoting = GMRoot.runner.economy.outputDetails.Single(x => x.type == GMData.Run.OutputDetail.TYPE.CHAOTING);
            //Assert.AreEqual(GMRoot.runner.chaoting.CalcTax(out_def.init_level, GMRoot.runner.chaoting.reportPopNum.Value), outputChaoting.Value.Value);
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = GMRoot.runner.Serialize();
            GMRoot.runner = GMData.Run.Runner.Deserialize(json);

            Test_Init();
        }

        [Test()]
        public void Test_RiskDaysInc()
        {
            //double? d = null;

            //double? s = d * 12;
        }
    }
}
