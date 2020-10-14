using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class Date
    {
        [SemanticProperty("year")]
        public int? year;

        [SemanticProperty("month")]
        public int? month;

        [SemanticProperty("day")]
        public int? day;

        public static Date Parse(SyntaxItem item)
        {
            if(item.values.Count == 1 && item.values[0] is StringValue)
            {
                var sValue = item.values[0] as StringValue;
                if (sValue.ToString() != "every_day")
                {
                    throw new Exception($"date with single value expect be 'every_day', but curr is {sValue.ToString()}");
                }

                return new Date();
            }

            return SemanticParser.DoParser<Date>(item);
        }

        public bool isTrue()
        {
            if (year != null && year != Visitor.GetValueFunc("date.year"))
            {
                return false;
            }
            if (month != null && month != Visitor.GetValueFunc("date.month"))
            {
                return false;
            }
            if (day != null && day != Visitor.GetValueFunc("date.day"))
            {
                return false;
            }

            return true;
        }
    }
}
