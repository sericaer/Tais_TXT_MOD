using System;
using System.Collections.Generic;
using Parser.Semantic;

namespace GMData.Def
{
    public class Economy
    {
        [SemanticProperty("curr")]
        public double curr;

        [SemanticPropertyArray("income")]
        public List<IncomeAdjust> incomes;

        [SemanticPropertyArray("output")]
        public List<OutputAdjust> outputs;

        //[SemanticProperty("income_percent_pop_tax")]
        //public double income_percent_pop_tax;

        //[SemanticProperty("output_percent_admin")]
        //public double output_percent_admin;

        //[SemanticProperty("output_percent_chaoting_tax")]
        //public double output_percent_chaoting_tax;
    }

    public class IncomeAdjust
    {
        public string key;
        public double percent;

        public double? effect_pop_tax;
        public double? effect_pop_consume;
    }

    public class OutputAdjust
    {
        public string key;
        public double percent;

        public double? effect_report_chaoting;
        public double? effect_spend_admin;
    }
}
