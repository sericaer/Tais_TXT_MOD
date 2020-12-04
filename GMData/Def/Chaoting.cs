using Parser.Semantic;
using System;
using System.Collections.Generic;

namespace GMData.Def
{
    public interface IChaoting
    {
        decimal reportPopPercent { get; set; }
        string powerParty { get; set; }
        TaxInfo taxInfo { get; set; }
    }

    public class TaxInfo
    {
        [SemanticProperty("init_level")]
        public int init_level;

        [SemanticPropertyArray("level_effect")]
        public List<TAX_LEVEL> levels;
    }

    public class TAX_LEVEL
    {
        [SemanticProperty("person")]
        public decimal per_person;
    }

    public class Chaoting : IChaoting
    {
        [SemanticProperty("report_pop_percent")]
        public decimal reportPopPercent { get; set; }

        [SemanticProperty("power_party")]
        public string powerParty { get; set; }

        [SemanticProperty("tax")]
        public TaxInfo taxInfo { get; set; }


    }
}
