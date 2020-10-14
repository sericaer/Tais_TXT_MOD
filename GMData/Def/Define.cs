using System.Collections.Generic;

namespace GMData.Def
{
    public class Define
    {
        public List<Risk> risks;
        public PersonName personName
        {
            get
            {
                return GMRoot.modder.personName;
            }
        }

        public List<Party> parties;

        public Dictionary<string, DepartDef> departs;
        public Dictionary<string, PopDef> pops;
        public Dictionary<string, PartyDef> partys;

        //public EconomyDef economy;
        public ChaotingDef chaoting;
        public CropDef crop;
        public List<TaxEffect> pop_tax;

        public Define()
        {
            departs = new Dictionary<string, DepartDef>()
                {
                    { "JIXIAN", new DepartDef() { color = (63, 72, 204),
                        pop_init = new (string name, int num)[] { ("haoqiang", 3000), ("minhu", 60000), ("yinhu", 20000) } } }
                };

            pops = new Dictionary<string, PopDef>()
                {
                    { "haoqiang", new PopDef() { is_collect_tax = true } },
                    { "minhu", new PopDef() { is_collect_tax = true, consume = 100} },
                    { "yinhu", new PopDef() { is_collect_tax = false } },
                };

            partys = new Dictionary<string, PartyDef>()
                {
                    { "shizu", new PartyDef() { name = "shizu"} },
                    { "huanguan", new PartyDef() { name = "huanguan"} }
                };

            //economy = new EconomyDef()
            //{
            //    curr = 456,
            //    pop_tax_percent = 30,
            //    expend_depart_admin = 100,
            //};


            chaoting = new ChaotingDef()
            {
                reportPopPercent = 130,
                taxPercent = 20,
                powerParty = "huanguan"
            };

            crop = new CropDef()
            {
                growSpeed = 0.4,
                growStartDay = (null, 2, 1),
                harvestDay = (null, 9, 1),
            };

            pop_tax = new List<TaxEffect>()
                {
                    new TaxEffect(){name = "level1", per_tax = 0.001, consume_effect = -10},
                    new TaxEffect(){name = "level2", per_tax = 0.002, consume_effect = -20},
                    new TaxEffect(){name = "level3", per_tax = 0.003, consume_effect = -30 },
                    new TaxEffect(){name = "level4", per_tax = 0.0035, consume_effect = -40 },
                    new TaxEffect(){name = "level5", per_tax = 0.004, consume_effect = -50 }
                };


            //risks = Risk.Load(path + "/risks/");
        }
    }

    public class DepartDef
    {
        public string name;
        public (int r, int g, int b) color;
        public (string name, int num)[] pop_init;
    }

    public class PopDef
    {
        public string name;
        public bool is_collect_tax;
        public double? consume;
    }

    //public class EconomyDef
    //{
    //    public double curr;
    //    public double pop_tax_percent;
    //    public double expend_depart_admin;
    //    public double report_chaoting_percent;
    //}

    public class ChaotingDef
    {
        public double reportPopPercent;
        public double taxPercent;
        public string powerParty;
    }

    public class CropDef
    {
        public double growSpeed;
        public (int?, int?, int?) growStartDay;
        public (int?, int?, int?) harvestDay;
    }

    public class TaxEffect
    {
        public string name;
        public double per_tax;
        public double consume_effect;
    }

    public class PartyDef
    {
        public string name;
    }
}