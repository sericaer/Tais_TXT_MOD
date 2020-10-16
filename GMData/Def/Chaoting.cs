using Parser.Semantic;
using System;
namespace GMData.Def
{
    public class Chaoting
    {

        [SemanticProperty("report_pop_percent")]
        public double reportPopPercent;

        [SemanticProperty("tax_percent")]
        public double taxPercent;

        [SemanticProperty("power_party")]
        public string powerParty;
    }
}
