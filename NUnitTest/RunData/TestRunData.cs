//using DataVisit;
//using Modder;
//using NUnit.Framework;
//using RunData;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//namespace UnitTest.RunData
//{
//    public class TestRunData
//    {
//        public InitData init;

//        public TestRunData()
//        {
//            Root.def = new Define.Def()
//            {
//                departs = new Dictionary<string, Define.DepartDef>()
//                {
//                    { "JIXIAN", new Define.DepartDef() { color = (63, 72, 204),
//                        pop_init = new (string name, int num)[] { ("haoqiang", 3000), ("minhu", 60000), ("yinhu", 20000) } } }
//                },

//                pops = new Dictionary<string, Define.PopDef>()
//                {
//                    { "haoqiang", new Define.PopDef() { is_collect_tax = true } },
//                    { "minhu", new Define.PopDef() { is_collect_tax = true, consume = 100} },
//                    { "yinhu", new Define.PopDef() { is_collect_tax = false } },
//                },

//                partys = new Dictionary<string, Define.PartyDef>()
//                {
//                    { "shizu", new Define.PartyDef() { name = "shizu"} },
//                    { "huanguan", new Define.PartyDef() { name = "huanguan"} }
//                },

//                economy = new Define.EconomyDef()
//                {
//                    curr = 456,
//                    pop_tax_percent = 30,
//                    report_chaoting_percent = 100,
//                    expend_depart_admin = 50,
//                },

//                chaoting = new Define.ChaotingDef()
//                {
//                    reportPopPercent = 110,
//                    taxPercent = 20,
//                    powerParty = "huanguan"
//                },

//                crop = new Define.CropDef()
//                {
//                    growSpeed = 0.4,
//                    growStartDay = (null, 2, 1),
//                    harvestDay = (null, 9, 1),
//                },

//                pop_tax = new List<Define.TaxEffect>()
//                {
//                    new Define.TaxEffect(){name = "level1", per_tax = 0.001, consume_effect = -10},
//                    new Define.TaxEffect(){name = "level2", per_tax = 0.002, consume_effect = -20},
//                    new Define.TaxEffect(){name = "level3", per_tax = 0.003, consume_effect = -30 },
//                    new Define.TaxEffect(){name = "level4", per_tax = 0.0035, consume_effect = -40 },
//                    new Define.TaxEffect(){name = "level5", per_tax = 0.004, consume_effect = -50 }
//                },

//                risks = new List<Define.Risk>()
//                {
//                    new Define.Risk(){key = "RISK1", cost_days = 30}
//                }
//            };

//            init = new InitData()
//            {
//                common = new InitData.Common()
//                {
//                    name = "TEST1",
//                    age = 34,
//                    party = "shizu"
//                }
//            };
//        }
//    }
//}
