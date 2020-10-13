//using DataVisit;
//using Modder;
//using NUnit.Framework;
//using RunData;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//namespace UnitTest.RunData
//{
//    [TestFixture()]
//    public class TestRoot : TestRunData
//    {
       
//        [Test()]
//        public void TestInitData()
//        {
//            Root.Init(init);

//            ModDataVisit.InitVisitData(Root.inst);

//            Visitor.InitVisitMap(typeof(Root));
//            Visitor.SetVisitData(Root.inst);

//            Assert.AreEqual(1, Visitor.Get("date.day"));
//            Assert.AreEqual(1, Visitor.Get("date.month"));
//            Assert.AreEqual(1, Visitor.Get("date.year"));

//            Assert.AreEqual(456, Visitor.Get("economy.value"));

//            Assert.AreEqual(init.common.name, Visitor.Get("taishou.name"));
//            Assert.AreEqual(init.common.age, Visitor.Get("taishou.age"));
//            Assert.AreEqual(init.common.party, Visitor.Get("taishou.party.type"));

//            //foreach (var income in Root.inst.economy.EnumerateInCome())
//            //{
//            //    switch (income.name)
//            //    {
//            //        case "STATIC_POP_TAX":
//            //            Assert.AreEqual(Root.def.economy.pop_tax_percent, income.percent.Value);
//            //            Assert.AreEqual(Pop.all.Sum(x => x.expectTax.Value), income.maxValue.Value);
//            //            Assert.AreEqual(income.maxValue.Value * income.percent.Value / 100, income.currValue.Value);
//            //            break;
//            //        default:
//            //            Assert.Fail();
//            //            break;

//            //    }
//            //}

//            //foreach (var output in Root.inst.economy.EnumerateOutput())
//            //{
//            //    switch (output.name)
//            //    {
//            //        case "STATIC_REPORT_CHAOTING_TAX":
//            //            Assert.AreEqual(Root.def.economy.report_tax_percent, output.percent.Value);
//            //            Assert.AreEqual(Chaoting.inst.currMonthTax.Value * output.percent.Value / 100, output.currValue.Value);
//            //            break;
//            //        default:
//            //            Assert.Fail();
//            //            break;

//            //    }
//            //}

//            foreach (var departName in Root.def.departs.Keys)
//            {
//                var departDef = Root.def.departs[departName];

//                var depart = Depart.GetByColor(departDef.color.r, departDef.color.g, departDef.color.b);
//                Assert.AreEqual(departName, depart.name);

//                Assert.AreEqual(departDef.pop_init.Where(x => Root.def.pops[x.name].is_collect_tax).Sum(x => x.num), depart.popNum.Value);

//                foreach (var init in departDef.pop_init)
//                {
//                    var pop = depart.pops.Single(x => x.name == init.name);
//                    Assert.AreEqual(init.num, pop.num.Value);
//                }
//            }

//            Visitor.Pos pos = null;
//            while (Visitor.EnumerateVisit("depart", ref pos))
//            {
//                Root.def.departs.ContainsKey(Visitor.Get("depart.name") as string);
//            }

//            Assert.AreEqual((int)(Depart.all.Sum(x => x.popNum.Value) * Root.def.chaoting.reportPopPercent / 100), Chaoting.inst.reportPopNum.Value);
//            //Assert.AreEqual(Root.def.chaoting.taxPercent, Chaoting.inst.taxPercent.Value);
//            //Assert.AreEqual(Chaoting.inst.reportPopNum.Value * 0.01 * Root.def.chaoting.taxPercent / 100, Chaoting.inst.currMonthTax.Value);
//        }

//        [SetUp()]
//        public void SetUp()
//        {

//        }

//        [Test()]
//        public void TestSetGetEconomyData()
//        {
//            Root.Init(init);

//            ModDataVisit.InitVisitData(Root.inst);

//            Visitor.InitVisitMap(typeof(Root));
//            Visitor.SetVisitData(Root.inst);

//            Visitor.Set("economy.value", 123.0);

//            Assert.AreEqual(123, Visitor.Get("economy.value"));

//            var reportTaxPercent = 50.0;

//            //Visitor.Set("economy.report_chaoting_tax_percent", reportTaxPercent);
//            //Assert.AreEqual(reportTaxPercent, Visitor.Get("economy.report_chaoting_tax_percent"));

//            //var reportChaotingTax = Root.inst.economy.EnumerateOutput().Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX");
//            //Assert.AreEqual(reportChaotingTax.maxValue.Value * reportTaxPercent / 100, reportChaotingTax.currValue.Value);

//            //var incomes = Root.inst.economy.EnumerateInCome();
//            //var popTax = incomes.Single(x => x.name == "STATIC_POP_TAX");

//            //Assert.AreEqual(Pop.all.Sum(x => x.expectTax.Value), popTax.maxValue.Value);
//            //Assert.AreEqual(popTax.maxValue.Value * 50 / 100, popTax.currValue.Value);

//        }

//        [Test()]
//        public void TestDateInc()
//        {
//            Root.Init(init);

//            ModDataVisit.InitVisitData(Root.inst);

//            Visitor.InitVisitMap(typeof(Root));
//            Visitor.SetVisitData(Root.inst);

//            for (int y = 1; y <= 10; y++)
//            {
//                for (int m = 1; m <= 12; m++)
//                {
//                    for (int d = 1; d <= 30; d++)
//                    {
//                        Assert.AreEqual(d, Visitor.Get("date.day"));
//                        Assert.AreEqual(m, Visitor.Get("date.month"));
//                        Assert.AreEqual(y, Visitor.Get("date.year"));

//                        Date.Inc();
//                    }
//                }
//            }

//            Assert.True(Date.inst == (11, null, null));
//            Assert.True(Date.inst == (11, null, 1));
//            Assert.True(Date.inst == (11, 1, 1));
//            Assert.True(Date.inst == (null, 1, 1));

//            Assert.True(Date.inst < (12, null, null));
//            Assert.True(Date.inst < (12, null, 1));
//            Assert.True(Date.inst < (12, 1, 1));
//            Assert.True(Date.inst < (null, 12, 30));
//            Assert.True(Date.inst < (null, 1, 30));

//            Assert.True(Date.inst > (10, null, null));
//            Assert.True(Date.inst > (10, 12, null));
//            Assert.True(Date.inst > (10, 12, 30));

//        }

//        //[Test()]
//        //public void TestChaotingDaysInc()
//        //{
//        //    Root.Init(init);

//        //    ModDataVisit.InitVisitData(Root.inst);

//        //    Visitor.InitVisitMap(typeof(Root));
//        //    Visitor.SetVisitData(Root.inst);

//        //    double yearExpertTax = 0;
//        //    double realYearTax = 0;

//        //    for (int y = 1; y <= 10; y++)
//        //    {
//        //        yearExpertTax = 0;
//        //        realYearTax = 0;

//        //        for (int m = 1; m <= 12; m++)
//        //        {
//        //            for (int d = 1; d <= 30; d++)
//        //            {
//        //                Chaoting.DaysInc();

//        //                if (d == 30)
//        //                {
//        //                    yearExpertTax += Chaoting.inst.currMonthTax.Value;
//        //                    realYearTax += Economy.inst.outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX").currValue.Value;
//        //                }

//        //                Assert.AreEqual(yearExpertTax, Chaoting.inst.expectYearTax.Value);
//        //                Assert.AreEqual(realYearTax, Chaoting.inst.realYearTax.Value);

//        //                Assert.AreEqual(Chaoting.inst.expectYearTax.Value, Visitor.Get("chaoting.expect_year_tax"));
//        //                Assert.AreEqual(Chaoting.inst.realYearTax.Value, Visitor.Get("chaoting.real_year_tax"));

//        //                Date.Inc();
//        //            }
//        //        }
//        //    }
//        //}

//        [Test()]
//        public void TestDepartDaysInc()
//        {
//            Root.Init(init);

//            ModDataVisit.InitVisitData(Root.inst);

//            Visitor.InitVisitMap(typeof(Root));
//            Visitor.SetVisitData(Root.inst);

//            double cropGrown = 0;

//            for (int y = 1; y <= 10; y++)
//            {
//                for (int m = 1; m <= 12; m++)
//                {
//                    for (int d = 1; d <= 30; d++)
//                    {
//                        if (Date.inst >= Root.def.crop.growStartDay && Date.inst <= Root.def.crop.harvestDay)
//                        {
//                            cropGrown += Root.def.crop.growSpeed;
//                        }
//                        else
//                        {
//                            cropGrown = 0;
//                        }

//                        Depart.DaysInc();

//                        foreach (var depart in Depart.all)
//                        {
//                            Assert.AreEqual(cropGrown, depart.cropGrown.Value);
//                        }

//                        Visitor.Pos pos = null;
//                        while (Visitor.EnumerateVisit("pop", ref pos))
//                        {
//                            Assert.AreEqual(cropGrown, Visitor.Get("pop.depart.crop_grown"));
//                        }

//                        Date.Inc();
//                    }
//                }
//            }
//        }

//        [Test()]
//        public void TestTaishou()
//        {
//            Root.Init(init);

//            ModDataVisit.InitVisitData(Root.inst);

//            Visitor.InitVisitMap(typeof(Root));
//            Visitor.SetVisitData(Root.inst);

//            Assert.AreEqual(false, Visitor.Get("taishou.is_revoke"));
//            Assert.AreEqual(false, Root.inst.isEnd);

//            Visitor.Set("taishou.is_revoke", true);
//            Assert.AreEqual(true, Visitor.Get("taishou.is_revoke"));
//            Assert.AreEqual(true, Root.inst.isEnd);
//        }

//        [Test()]
//        public void TestSerialize()
//        {
//            Root.Init(init);

//            ModDataVisit.InitVisitData(Root.inst);

//            //COLLECT_POP_TAX.Start();

//            var json = Root.Serialize();

//            Root.Deserialize(json);

//            Visitor.InitVisitMap(typeof(Root));
//            Visitor.SetVisitData(Root.inst);

//            Assert.AreEqual(1, Visitor.Get("date.day"));
//            Assert.AreEqual(1, Visitor.Get("date.month"));
//            Assert.AreEqual(1, Visitor.Get("date.year"));

//            Assert.AreEqual(456, Visitor.Get("economy.value"));

//            //foreach (var income in Root.inst.economy.EnumerateInCome())
//            //{
//            //    switch (income.name)
//            //    {
//            //        case "STATIC_POP_TAX":
//            //            Assert.AreEqual(Root.def.economy.pop_tax_percent, income.percent.Value);
//            //            Assert.AreEqual(Pop.all.Sum(x => x.expectTax.Value), income.maxValue.Value);
//            //            Assert.AreEqual(income.maxValue.Value * income.percent.Value / 100, income.currValue.Value);
//            //            break;
//            //        default:
//            //            Assert.Fail();
//            //            break;

//            //    }
//            //}

//            //foreach (var output in Root.inst.economy.EnumerateOutput())
//            //{
//            //    switch (output.name)
//            //    {
//            //        case "STATIC_REPORT_CHAOTING_TAX":
//            //            Assert.AreEqual(Root.def.economy.report_tax_percent, output.percent.Value);
//            //            Assert.AreEqual(Chaoting.inst.currMonthTax.Value * output.percent.Value / 100, output.currValue.Value);
//            //            break;
//            //        default:
//            //            Assert.Fail();
//            //            break;

//            //    }
//            //}

//            foreach (var departName in Root.def.departs.Keys)
//            {
//                var departDef = Root.def.departs[departName];

//                var depart = Depart.GetByColor(departDef.color.r, departDef.color.g, departDef.color.b);
//                Assert.AreEqual(departName, depart.name);

//                Assert.AreEqual(departDef.pop_init.Where(x => Root.def.pops[x.name].is_collect_tax).Sum(x => x.num), depart.popNum.Value);

//                foreach (var init in departDef.pop_init)
//                {
//                    var pop = depart.pops.Single(x => x.name == init.name);
//                    Assert.AreEqual(init.num, pop.num.Value);
//                }
//            }

//            Assert.AreEqual((int)(Depart.all.Sum(x => x.popNum.Value) * Root.def.chaoting.reportPopPercent / 100), Chaoting.inst.reportPopNum.Value);
//            //Assert.AreEqual(Root.def.chaoting.taxPercent, Chaoting.inst.taxPercent.Value);
//            //Assert.AreEqual(Chaoting.inst.reportPopNum.Value * 0.01 * Root.def.chaoting.taxPercent / 100, Chaoting.inst.currMonthTax.Value);
//        }
//    }
//}
