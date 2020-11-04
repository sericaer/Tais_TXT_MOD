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

                            valid = true,

                            init_level = 3,

                            levels = new List<IncomeAdjust.Level>()
                            {
                                new IncomeAdjust.Level()
                                {
                                    percent = 0,
                                    //effect_pop_consume = -2,
                                },
                                new IncomeAdjust.Level()
                                {
                                    percent = +5,
                                    effect_pop_consume = -5,
                                },
                                new IncomeAdjust.Level()
                                {
                                    percent = +10,
                                    effect_pop_consume = -10,
                                },
                                new IncomeAdjust.Level()
                                {
                                    percent = +20,
                                    effect_pop_consume = -15,
                                },
                                new IncomeAdjust.Level()
                                {
                                    percent = +30,
                                    effect_pop_consume = -20,
                                },
                                new IncomeAdjust.Level()
                                {
                                    percent = +40,
                                    effect_pop_consume = -30,
                                },
                                new IncomeAdjust.Level()
                                {
                                    percent = +50,
                                    effect_pop_consume = -40,
                                }
                            }
                        }
                    },

                    outputs = new List<OutputAdjust>()
                    {
                        new OutputAdjust()
                        {
                            key = "ADMIN",

                            valid = true,

                            init_level = 3,

                            levels = new List<OutputAdjust.Level>()
                            {
                                new OutputAdjust.Level()
                                {
                                    percent = -50
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = -40
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = -30
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = -20
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = -10
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = 0
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = +10
                                }
                            }
                        }, 
                        new OutputAdjust()
                        {
                            key = "CHAOTING",

                            valid = false,

                            init_level = 3,

                            levels = new List<OutputAdjust.Level>()
                            {
                                new OutputAdjust.Level()
                                {
                                    percent = -100
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = -60
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = -30
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = 0
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = 10
                                },
                                new OutputAdjust.Level()
                                {
                                    percent = 20
                                },
                                new OutputAdjust.Level()
                                {
                                   percent = 30
                                }
                            }
                        }
                    }
                },

                chaoting = new Chaoting()
                {
                    reportPopPercent = 110,
                    powerParty = "TEST_PARTY",
                    tax_level = 3,
                },

                risks = new List<Risk>()
                {
                    new Risk(){ key = "TEST_RISK_0", cost_days = 100}
                }
            };
        }
    }
}
