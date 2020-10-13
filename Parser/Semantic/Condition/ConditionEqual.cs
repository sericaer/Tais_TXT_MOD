﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class ConditionEqual : ConditionTuple
    {
        public ConditionEqual(SyntaxItem item) : base(item)
        {

        }

        public override bool Rslt()
        {
            return Visitor.GetValue(right) == Visitor.GetValue(left);
        }
    }
}
