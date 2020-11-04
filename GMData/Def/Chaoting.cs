using Parser.Semantic;
using System;
namespace GMData.Def
{
    public class Chaoting
    {
        [SemanticProperty("report_pop_percent")]
        public double reportPopPercent;

        [SemanticProperty("power_party")]
        public string powerParty;

        [SemanticProperty("tax_level")]
        public int tax_level;
    }
}
