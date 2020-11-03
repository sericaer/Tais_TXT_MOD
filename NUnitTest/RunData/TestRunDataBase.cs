using DataVisit;
using GMData;
using GMData.Def;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    public class TestRunDataBase
    {
        public TestRunDataBase()
        {
            GMRoot.initData = new GMData.Init.InitData()
            {
                start_date = new GMData.Init.Date()
                {
                    day = 1,
                    month = 1,
                    year = 1
                },

                taishou = new GMData.Init.Taishou()
                {
                    name = "TEST_NAME",
                    age = 123,
                    party = "TEST_PARTY"
                }
            };

            GMRoot.define = new Define()
            {
                parties = new List<Party>() { new Party("TEST_PARTY") },

                departs = new List<Depart>()
                {
                    new Depart()
                    {
                        key = "DEPART1",
                        color = new Depart.COLOR(){ r=1, g=1, b=1},
                        pop_init = new List<Depart.POP_INIT>()
                        {
                            new Depart.POP_INIT(){type = "POP_1", num = 12345},
                            new Depart.POP_INIT(){type = "POP_2", num = 67890}
                        }
                    },
                    new Depart()
                    {
                        key = "DEPART2",
                        color = new Depart.COLOR(){ r=2, g=2, b=2},
                        pop_init = new List<Depart.POP_INIT>()
                        {
                            new Depart.POP_INIT(){type = "POP_1", num = 1234},
                            new Depart.POP_INIT(){type = "POP_2", num = 6789},
                            new Depart.POP_INIT(){type = "POP_3", num = 200}
                        }
                    }
                },

                pops = new List<Pop>()
                {
                    new Pop() {key = "POP_1", is_collect_tax = true },
                    new Pop() {key = "POP_2", is_collect_tax = true, consume = 100} ,
                    new Pop() {key = "POP_3", is_collect_tax = false },
                },

                economy = new Economy()
                {
                    curr = 123,

                    incomes = new List<IncomeAdjust>()
                    {
                        new IncomeAdjust
                        {
                            key = "POP_TAX",
                            percent = 0.3,
                            effect_pop_tax = +2,
                            effect_pop_consume = -0.5,
                        }
                    },

                    outputs = new List<OutputAdjust>()
                    {
                        new OutputAdjust()
                        {
                            key = "ADMIN",
                            percent = 0.8,
                        }, 
                        new OutputAdjust()
                        {
                            key = "CHAOTING",
                            percent = 1,
                            effect_report_chaoting = +1,
                            effect_spend_admin = +1
                        }
                    }

                    //income_percent_pop_tax = 30,
                    //output_percent_admin = 80,
                    //output_percent_chaoting_tax = 100
                },

                chaoting = new Chaoting()
                {
                    reportPopPercent = 110,
                    powerParty = "TEST_PARTY",
                    taxPercent = 20,
                },

                risks = new List<Risk>()
                {
                    new Risk(){ key = "TEST_RISK_0", cost_days = 100}
                }
            };
        }
    }
}
