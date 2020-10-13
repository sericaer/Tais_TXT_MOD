using Parser.Syntax;

namespace Parser.Semantic
{
    internal class ConditionGreater : ConditionTuple
    {
        public ConditionGreater(SyntaxItem item) : base(item)
        {

        }

        public override bool Rslt()
        {
            return Visitor.GetValue(right) > Visitor.GetValue(left);
        }
    }
}