using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public abstract class Condition
    {
        public static Condition Parse(SyntaxItem item)
        {
            try
            {
                if (item.values.Count() != 1)
                {
                    throw new Exception($"have more than one values");
                }

                var bValue = item.values[0] as BoolValue;
                if (bValue != null)
                {
                    return new ConditionDefault(bValue.data);
                }

                var subItem = item.values[0] as SyntaxItem;
                if (subItem == null)
                {
                    throw new Exception($"not support value type");
                }

                return ParseItem(subItem);
            }
            catch (Exception e)
            {
                throw new Exception($"Parse conditon faild! key:{item.key}", e);
            }

        }

        internal static Condition ParseItem(SyntaxItem item)
        {
            switch (item.key)
            {
                case "equal":
                    return new ConditionEqual(item);
                case "less":
                    return new ConditionLess(item);
                case "greater":
                    return new ConditionGreater(item);
                case "or":
                    return new ConditionOr(item);
                case "and":
                    return new ConditionAnd(item);
                case "not":
                    return new ConditionNot(item);
                default:
                    throw new Exception($"not support item '{item.key}'");
            }
        }

        public abstract bool Rslt();
    }
}
