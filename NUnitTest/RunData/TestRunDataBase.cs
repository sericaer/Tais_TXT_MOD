using DataVisit;
using GMData;
using GMData.Def;
using GMData.Mod;
using NUnit.Framework;
using Parser.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    public class CHAOTING_DEF_MOCK : GMData.Def.IChaoting
    {
        public decimal reportPopPercent { get; set; }
        public string powerParty { get; set; }
        public TaxInfo taxInfo { get; set; }
    }

    public class RISK_DEF_MOCK : GMData.Def.IRisk
    {
        public string key { get; set; }
        public decimal cost_days { get; set; }
        public string endEvent { get; set; }
        public IRandomEvents randomEvent { get; set; }

        public Func<GMData.Run.Risk, IGEvent> CalcEndEvent { get; set; }

        public IChoice[] options { get; set; }

        public RISK_DEF_MOCK()
        {
            CalcEndEvent = _ =>
            {
                return null;
            };
        }
    }

    public class GEvent_MOCK : GMData.Mod.IGEvent
    {
        public string key { get; set; }

        public Title title { get; set; }
        public GMData.Mod.Desc desc { get; set; }
        public GMData.Mod.Option[] options { get; set; }

        public Tuple<string, object> objTuple { get; set; }

        public Func<string, IGEvent> GetNext { get; set; }
    }

    public class TestRunDataBase
    {
        public TestRunDataBase()
        {
            //GMRoot.AssocTypeStatic();

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
                parties = new List<Party>() {
                    new Party()
                    {
                        key = "TEST_PARTY1",
                        relation = new List<Party.Relation>()
                        {
                            new Party.Relation()
                            {
                                peer = "TEST_PARTY2",
                                value = 100
                            },
                            new Party.Relation()
                            {
                                peer = "TEST_PARTY1",
                                value = 30
                            }
                        }
                    },
                    new Party()
                    {
                        key = "TEST_PARTY2",
                        relation = new List<Party.Relation>()
                        {
                            new Party.Relation()
                            {
                                peer = "TEST_PARTY2",
                                value = -10
                            },
                            new Party.Relation()
                            {
                                peer = "TEST_PARTY1",
                                value = -40
                            }
                        }
                    }
                },
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
                },

                risks = new List<Risk>()
                {
                    new Risk() { key = "TEST_RISK_0", cost_days = 100 },
                    new Risk() { key = "TEST_RISK_1", cost_days = 200, endEvent = "EVENT_END_TEST_RISK_1"}
                },

                personName = new PersonName(new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" },
                                           new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" })
            };
        }
    }
}
