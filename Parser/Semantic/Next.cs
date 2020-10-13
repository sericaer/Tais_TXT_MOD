using System;
using System.Collections.Generic;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class Next
    {
        public static Next Parse(SyntaxItem item)
        {
            return new Next(item.values);
        }

        public string Rslt()
        {
            foreach(var sub in list)
            {
                if(sub.cond.Rslt())
                {
                    return sub.name;
                }
            }

            return null;
        }

        List<(string name, Condition cond)> list;

        public Next(List<Value> values)
        {
            list = new List<(string name, Condition cond)>();

            string defaultKey = null;

            foreach (var value in values)
            {
                var strItem = value as StringValue;
                if(strItem != null)
                {
                    list.Add((strItem.data, new ConditionDefault(true)));
                    return;
                }

                var subItem = value as SyntaxItem;
                if (subItem == null)
                {
                    throw new Exception($"next only support string value type or item type");
                }

                if (subItem.key == "default")
                {
                    if(subItem.values.Count != 1)
                    {
                        throw new Exception($"next default only support string value");
                    }

                    var defaultValue = subItem.values[0] as StringValue;
                    if (defaultValue == null)
                    {
                        throw new Exception($"next default only support string value");
                    }

                    defaultKey = defaultValue.data;
                    continue;
                }

                list.Add((subItem.key, Condition.Parse(subItem)));
            }

            if(defaultKey != null)
            {
                list.Add((defaultKey, new ConditionDefault(true)));
            }
        }
    }
}