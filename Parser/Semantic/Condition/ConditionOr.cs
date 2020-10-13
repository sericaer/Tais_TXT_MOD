﻿using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Syntax;

namespace Parser.Semantic
{
    internal class ConditionOr : Condition
    {

        public ConditionOr(SyntaxItem item)
        {
            subList = new List<Condition>();

            foreach (var value in item.values)
            {
                var subItem = value as SyntaxItem;
                if (subItem == null)
                {
                    throw new Exception($"not support value type");
                }

                subList.Add(Condition.ParseItem(subItem));
            }
        }

        public override bool Rslt()
        {
            return subList.Any(x => x.Rslt());
        }

        private List<Condition> subList;
    }
}