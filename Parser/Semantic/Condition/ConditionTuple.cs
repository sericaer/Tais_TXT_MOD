using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public abstract class ConditionTuple : Condition
    {
        internal ConditionTuple(SyntaxItem item)
        {
            if(item == null && this is ConditionDefault)
            {
                return;
            }

            if (item.values.Count() != 2)
            {
                throw new Exception("conditon equal must have 2 values!");
            }

            var value0 = item.values[0] as SingleValue;
            var value1 = item.values[1] as SingleValue;
            if (value0 == null || value1 == null)
            {
                throw new Exception("conditon equal value must be SingleValue");
            }

            right = value0;
            left = value1;
        }

        internal SingleValue right;
        internal SingleValue left;
    }
}
