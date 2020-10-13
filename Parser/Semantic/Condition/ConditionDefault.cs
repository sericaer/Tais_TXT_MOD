using System;
namespace Parser.Semantic
{
    public class ConditionDefault : Condition
    {
        public ConditionDefault(bool defaultValue) : base()
        {
            this.defaultValue = defaultValue;
        }

        public override bool Rslt()
        {
            return defaultValue;
        }

        private bool defaultValue;
    }
}
