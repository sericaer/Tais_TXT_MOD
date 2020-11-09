using System.Collections.Generic;

namespace GMData.Def
{
    public class Define
    {
        public PersonName personName;
        public List<Party> parties;
        public List<Depart> departs;
        public List<Pop> pops;
        public Economy economy;
        public Chaoting chaoting;

        public List<Adjust> adjusts;

        public List<Risk> risks;
        //public Dictionary<string, PopDef> pops;
        public Dictionary<string, PartyDef> partys;

        //public EconomyDef economy;
        public CropDef crop;
        public List<TaxEffect> pop_tax;
       
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