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
                    party = "TEST_PARTY1"
                }
            };

            GMRoot.define = new Define()
            {
                parties = new List<Party>() { new Party("TEST_PARTY1"), new Party("TEST_PARTY2") },

                departs = new List<Depart>()
                {
                    new Depart()
                    {
                        key = "DEPART1",
                        color = new Depart.COLOR() { r = 1, g = 1, b = 1 },
                        pop_init = new List<Depart.POP_INIT>()
                        {
                            new Depart.POP_INIT() { type = "POP_1", num = 321, party = "TEST_PARTY1" },
                            new Depart.POP_INIT() { type = "POP_2", num = 67890 }
                        }
                    },
                    new Depart()
                    {
                        key = "DEPART2",
                        color = new Depart.COLOR() { r = 2, g = 2, b = 2 },
                        pop_init = new List<Depart.POP_INIT>()
                        {
                            new Depart.POP_INIT() { type = "POP_1", num = 1234, party ="TEST_PARTY2" },
                            new Depart.POP_INIT() { type = "POP_2", num = 6789 },
                            new Depart.POP_INIT() { type = "POP_3", num = 200 }
                        }
                    }
                },

                pops = new List<Pop>()
                {
                    new Pop() { key = "POP_1", is_collect_tax = true, is_family = true },
                    new Pop() { key = "POP_2", is_collect_tax = true, consume = 100 },
                    new Pop() { key = "POP_3", is_collect_tax = false },
                },

                economy = new Economy()
                {
                    curr = 123,
                },

                adjusts = new List<Adjust>()
                {
                    new Adjust()
                    {
                        key = "POP_TAX",

                        init = new Adjust.Init()
                        {
                            valid = true,
                            level = 3,
                        },

                        levels = new List<Adjust.Level>()
                        {
                            new Adjust.Level()
                            {
                                percent = 0,
                                //effect_pop_consume = -2,
                            },
                            new Adjust.Level()
                            {
                                percent = +5,
                                effect_pop_consume = -5,
                            },
                            new Adjust.Level()
                            {
                                percent = +10,
                                effect_pop_consume = -10,
                            },
                            new Adjust.Level()
                            {
                                percent = +20,
                                effect_pop_consume = -15,
                            },
                            new Adjust.Level()
                            {
                                percent = +30,
                                effect_pop_consume = -20,
                            },
                            new Adjust.Level()
                            {
                                percent = +40,
                                effect_pop_consume = -30,
                            },
                            new Adjust.Level()
                            {
                                percent = +50,
                                effect_pop_consume = -40,
                            }
                        }
                    },
                    new Adjust()
                    {
                        key = "ADMIN_SPEND",


                        init = new Adjust.Init()
                        {
                            valid = true,
                            level = 3,
                        },

                        levels = new List<Adjust.Level>()
                        {
                            new Adjust.Level()
                            {
                                percent = -50
                            },
                            new Adjust.Level()
                            {
                                percent = -40
                            },
                            new Adjust.Level()
                            {
                                percent = -30
                            },
                            new Adjust.Level()
                            {
                                percent = -20
                            },
                            new Adjust.Level()
                            {
                                percent = -10
                            },
                            new Adjust.Level()
                            {
                                percent = 0
                            },
                            new Adjust.Level()
                            {
                                percent = +10
                            }
                        }
                    },
                    new Adjust()
                    {
                        key = "REPORT_CHAOTING",

                        init = new Adjust.Init()
                        {
                            valid = true,
                            level = 3,
                        },

                        levels = new List<Adjust.Level>()
                        {
                            new Adjust.Level()
                            {
                                percent = -100
                            },
                            new Adjust.Level()
                            {
                                percent = -60
                            },
                            new Adjust.Level()
                            {
                                percent = -30
                            },
                            new Adjust.Level()
                            {
                                percent = 0
                            },
                            new Adjust.Level()
                            {
                                percent = 10
                            },
                            new Adjust.Level()
                            {
                                percent = 20
                            },
                            new Adjust.Level()
                            {
                                percent = 30
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
                    new Risk() { key = "TEST_RISK_0", cost_days = 100 }
                },

                personName = new PersonName(new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" },
                                           new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" })
            };
        }
    }
}
