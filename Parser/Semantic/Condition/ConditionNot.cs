using System;
using System.Linq;
using Parser.Syntax;

namespace Parser.Semantic
{
    internal class ConditionNot : Condition
    {
        public ConditionNot(SyntaxItem item)
        {
            sub = Condition.Parse(item);
        }

        public override bool Rslt()
        {
            return !sub.Rslt();
        }

        private Condition sub;
    }
}