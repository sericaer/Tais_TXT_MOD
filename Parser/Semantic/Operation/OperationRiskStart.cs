using System;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class OperationRiskStart : Operation
    {
        internal static Operation Parse(SyntaxItem subItem)
        {
            if (subItem.values.Count != 1
                || !(subItem.values[0] is StringValue))
            {
                throw new Exception("add risk_start iteam must be have one SingleValue!");
            }

            return new OperationRiskStart() { left = new StringValue() { data = "risk.start"}, right = subItem.values[0] as SingleValue };
        }

        public override void Do()
        {
            Visitor.SetValue(left.ToString(), right.ToString());
        }
    }
}
