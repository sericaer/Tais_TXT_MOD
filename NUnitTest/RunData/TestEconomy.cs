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
    public class TestEconomy : TestRunDataBase
    {
        private GMData.Run.Economy economy;

        [SetUp]
        public void Init()
        {
            //GMRoot.runner = GMData.Run.Runner.Generate();
            economy = new GMData.Run.Economy(GMRoot.define.economy);
            economy.incomeDetails.ForEach(x => x.Value.OnNext(100));
            economy.outputDetails.ForEach(x => x.Value.OnNext(200));
        }

        [Test()]
        public void Test_Init()
        {
            Assert.AreEqual(GMRoot.define.economy.curr, economy.curr.Value);

            Assert.AreEqual(GMRoot.define.economy.incomes.Count(), economy.incomeDetails.Count());
            Assert.AreEqual(economy.incomeDetails.Count() * 100, economy.incomeTotal.Value);

            Assert.AreEqual(GMRoot.define.economy.outputs.Count(), economy.outputDetails.Count());
            Assert.AreEqual(economy.outputDetails.Count() * 200, economy.outputTotal.Value);

            Assert.AreEqual(economy.incomeTotal.Value - economy.outputTotal.Value, economy.monthSurplus.Value);

            foreach (var income_def in GMRoot.define.economy.incomes)
            {
                var incomeObj = economy.incomeAdjusts.SingleOrDefault(x => x.def == income_def);
                Assert.NotNull(incomeObj);

                Assert.AreEqual(income_def.key, incomeObj.key);
                Assert.AreEqual(income_def.percent, incomeObj.percent.Value);

                if(income_def.effect_pop_consume == null)
                {
                    Assert.Null(incomeObj.effect_pop_consume);
                }
                else
                {
                    Assert.AreEqual(income_def.effect_pop_consume * income_def.percent, incomeObj.effect_pop_consume.Value);
                }

                if (income_def.effect_pop_tax == null)
                {
                    Assert.Null(incomeObj.effect_pop_tax);
                }
                else
                {
                    Assert.AreEqual(income_def.effect_pop_tax * income_def.percent, incomeObj.effect_pop_tax.Value);
                }
            }

            foreach (var output_def in GMRoot.define.economy.outputs)
            {
                var outputObj = economy.outputAdjusts.SingleOrDefault(x => x.def == output_def);
                Assert.NotNull(outputObj);

                Assert.AreEqual(output_def.key, outputObj.key);
                Assert.AreEqual(output_def.percent, outputObj.percent.Value);

                if (output_def.effect_report_chaoting == null)
                {
                    Assert.Null(outputObj.effect_report_chaoting);
                }
                else
                {
                    Assert.AreEqual(output_def.effect_report_chaoting * output_def.percent, outputObj.effect_report_chaoting.Value);
                }

                if (output_def.effect_spend_admin == null)
                {
                    Assert.Null(outputObj.effect_spend_admin);
                }
                else
                {
                    Assert.AreEqual(output_def.effect_spend_admin * output_def.percent, outputObj.effect_spend_admin.Value);
                }
            }
        }

        [Test()]
        public void Test_EconomyDayInc()
        {
            var date = new GMData.Run.Date(GMRoot.initData.start_date);
            
            var curr = GMRoot.define.economy.curr;
            for (int i=1; i<=360*10; i++)
            {
                if (i % 30 == 0)
                {
                    curr += economy.monthSurplus.Value;
                }
                
                economy.DaysInc(date);
                date.Inc();

                Assert.AreEqual(curr, economy.curr.Value);
            }
        }

        [Test()]
        public void Test_Serialize()
        {
            var json = JsonConvert.SerializeObject(economy, Formatting.Indented);
            economy = JsonConvert.DeserializeObject<GMData.Run.Economy>(json);

            economy.incomeDetails.ForEach(x => x.Value.OnNext(100));
            economy.outputDetails.ForEach(x => x.Value.OnNext(200));

            Test_Init();
        }

        //    [Test()]
        //    public void Test_EconomyCurrValue()
        //    {
        //        ModDataVisit.InitVisitMap(typeof(Root));

        //        Root.Init(init);
        //        ModDataVisit.InitVisitData(Root.inst);

        //        Assert.AreEqual(Root.def.economy.curr, Visitor.Get("economy.value"));

        //        Economy.inst.curr.Value = 100;

        //        var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

        //        Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

        //        Assert.AreEqual(100, Visitor.Get("economy.value"));
        //    }

        //    [Test()]
        //    public void Test_EconomyInCome_PopTax()
        //    {
        //        ModDataVisit.InitVisitMap(typeof(Root));

        //        Root.Init(init);
        //        ModDataVisit.InitVisitData(Root.inst);

        //        var popTax = Economy.inst.incomes.Single(x => x.name == "STATIC_POP_TAX");

        //        Assert.AreEqual(Root.def.economy.pop_tax_percent, popTax.percent.Value);
        //        Assert.AreEqual(Depart.all.Sum(x => x.tax.Value), popTax.maxValue.Value);
        //        Assert.AreEqual(popTax.maxValue.Value * popTax.percent.Value / 100, popTax.currValue.Value);

        //        popTax.percent.Value = 12.3;

        //        var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

        //        Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

        //        Assert.AreEqual(12.3, popTax.percent.Value);
        //        Assert.AreEqual(Depart.all.Sum(x => x.tax.Value), popTax.maxValue.Value);
        //        Assert.AreEqual(popTax.maxValue.Value * popTax.percent.Value / 100, popTax.currValue.Value);
        //    }

        //    [Test()]
        //    public void Test_EconomyOutput_AdminExpend()
        //    {
        //        ModDataVisit.InitVisitMap(typeof(Root));

        //        Root.Init(init);
        //        ModDataVisit.InitVisitData(Root.inst);

        //        var adminExpend = Economy.inst.outputs.Single(x => x.name == "STATIC_ADMIN_EXPEND");

        //        Assert.AreEqual(Root.def.economy.expend_depart_admin, adminExpend.percent.Value);
        //        Assert.AreEqual(Depart.all.Sum(x => x.adminExpendBase.Value), adminExpend.maxValue.Value);
        //        Assert.AreEqual(adminExpend.maxValue.Value * adminExpend.percent.Value / 100, adminExpend.currValue.Value);

        //        adminExpend.percent.Value = 12.3;

        //        var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

        //        Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

        //        Assert.AreEqual(12.3, adminExpend.percent.Value);
        //        Assert.AreEqual(Depart.all.Sum(x => x.adminExpendBase.Value), adminExpend.maxValue.Value);
        //        Assert.AreEqual(adminExpend.maxValue.Value * adminExpend.percent.Value / 100, adminExpend.currValue.Value);
        //    }

        //    [Test()]
        //    public void Test_EconomyOutput_ReportChaoting()
        //    {
        //        ModDataVisit.InitVisitMap(typeof(Root));

        //        Root.Init(init);
        //        ModDataVisit.InitVisitData(Root.inst);

        //        var report = Economy.inst.outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX");

        //        Assert.AreEqual(Root.def.economy.report_chaoting_percent, report.percent.Value);
        //        Assert.AreEqual(Chaoting.inst.expectMonthTaxValue.Value, report.maxValue.Value);
        //        Assert.AreEqual(report.maxValue.Value * report.percent.Value / 100, report.currValue.Value);

        //        report.percent.Value = 12.3;

        //        var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

        //        Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

        //        Assert.AreEqual(12.3, report.percent.Value);
        //        Assert.AreEqual(Chaoting.inst.expectMonthTaxValue.Value, report.maxValue.Value);
        //        Assert.AreEqual(report.maxValue.Value * report.percent.Value / 100, report.currValue.Value);

        //        Date.inst.day.Value = 30;
        //        Economy.DaysInc();

        //        Assert.AreEqual(Chaoting.inst.expectMonthTaxValue.Value - report.currValue.Value, Chaoting.inst.oweTax);
        //    }

        //    [Test()]
        //    public void Test_EconomyMonthSurplus()
        //    {
        //        ModDataVisit.InitVisitMap(typeof(Root));

        //        Root.Init(init);
        //        ModDataVisit.InitVisitData(Root.inst);

        //        Assert.AreEqual(Economy.inst.incomes.Sum(x => x.currValue.Value), Economy.inst.incomes.total.Value);
        //        Assert.AreEqual(Economy.inst.outputs.Sum(x => x.currValue.Value), Economy.inst.outputs.total.Value);

        //        Assert.AreEqual(Economy.inst.incomes.total.Value - Economy.inst.outputs.total.Value, Economy.inst.monthSurplus.Value);

        //    }

        //    [Test()]
        //    public void Test_EconomyDayInc()
        //    {
        //        ModDataVisit.InitVisitMap(typeof(Root));

        //        Root.Init(init);
        //        ModDataVisit.InitVisitData(Root.inst);

        //        Date.inst.day.Value = 29;
        //        Economy.DaysInc();

        //        Assert.AreEqual(Root.def.economy.curr, Visitor.Get("economy.value"));

        //        Date.inst.day.Value = 30;
        //        Economy.DaysInc();

        //        Assert.AreEqual(Root.def.economy.curr + Economy.inst.monthSurplus.Value, Visitor.Get("economy.value"));
        //    }
    }
}
