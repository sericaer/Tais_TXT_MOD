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

            foreach (var adjust_def in GMRoot.define.economy.incomes.Where(x => x.effect_pop_tax != null))
            {
                foreach(var tax in GMRoot.runner.pops.SelectNotNull(x=>x.tax))
                {
                    Assert.True(tax.buffers.Contains(adjust_def.key));
                    Assert.AreEqual(adjust_def.effect_pop_tax * adjust_def.percent * tax.baseValue.Value, tax.buffers[adjust_def.key]);
                    Assert.AreEqual(tax.baseValue.Value + tax.buffers.Values.Cast<double>().Sum(), tax.value.Value);
                }
            }

            foreach (var adjust_def in GMRoot.define.economy.outputs)
            {
                if(adjust_def.effect_spend_admin != null)
                {
                    foreach (var admin in GMRoot.runner.pops.SelectNotNull(x => x.adminExpend))
                    {
                        Assert.True(admin.buffers.Contains(adjust_def.key));
                        Assert.AreEqual(adjust_def.effect_spend_admin * adjust_def.percent * admin.baseValue.Value, admin.buffers[adjust_def.key]);
                        Assert.AreEqual(admin.baseValue.Value + admin.buffers.Values.Cast<double>().Sum(), admin.value.Value);
                    }
                }
                if(adjust_def.effect_report_chaoting != null)
                {
                    //foreach (var report in GMRoot.runner.pops.SelectNotNull(x => x.adminExpend))
                    //{
                    //    Assert.True(admin.buffers.Contains(adjust_def.key));
                    //    Assert.AreEqual(adjust_def.effect_spend_admin * adjust_def.percent * admin.baseValue.Value, admin.buffers[adjust_def.key]);
                    //    Assert.AreEqual(admin.baseValue.Value + admin.buffers.Values.Cast<double>().Sum(), admin.value.Value);
                    //}

                }
            }

            var incomePopTax = GMRoot.runner.economy.incomeDetails.Single(x => x.type == GMData.Run.IncomeDetail.TYPE.POP_TAX);
            Assert.AreEqual(GMRoot.runner.pops.Sum(x=>x.tax?.value.Value), incomePopTax.Value.Value);

            var outputPopAdmin = GMRoot.runner.economy.outputDetails.Single(x => x.type == GMData.Run.OutputDetail.TYPE.ADMIN);
            Assert.AreEqual(GMRoot.runner.pops.Sum(x => x.adminExpend?.value.Value), outputPopAdmin.Value.Value);

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

        }
    }
}
