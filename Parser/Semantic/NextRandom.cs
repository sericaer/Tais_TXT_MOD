using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Syntax;

namespace Parser.Semantic
{
    public interface IRandomEvents
    {
        IEnumerable<(string name, decimal value)> Calc();
    }

    public class RandomEvents : IRandomEvents
    {
        public static RandomEvents Parse(SyntaxItem item)
        {
            return new RandomEvents(item.values);
        }

        public IEnumerable<(string name, decimal value)> Calc()
        {
            return list.Select(x => (x.name, x.modifier.CalcValue()));
        }

        internal List<(string name, ModifierGroup modifier)> list;

        public RandomEvents(List<Value> values)
        {
            list = new List<(string name, ModifierGroup cond)>();

            foreach (var value in values)
            {
                var subItem = value as SyntaxItem;
                if (subItem == null)
                {
                    throw new Exception($"not support value type");
                }

                list.Add((subItem.key, ModifierGroup.Parse(subItem)));
            }
        }
    }
}