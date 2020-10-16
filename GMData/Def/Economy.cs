using System;
using Parser.Semantic;

namespace GMData.Def
{
    public class Economy
    {
        [SemanticProperty("curr")]
        public double curr;

        [SemanticProperty("income_percent_pop_tax")]
        public double income_percent_pop_tax;

        [SemanticProperty("output_percent_admin")]
        public double output_percent_admin;

        [SemanticProperty("output_percent_chaoting_tax")]
        public double output_percent_chaoting_tax;
    }
}
