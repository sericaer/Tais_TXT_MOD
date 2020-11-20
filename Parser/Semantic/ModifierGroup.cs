using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class ModifierGroup
    {
        public static ModifierGroup Parse(SyntaxItem item)
        {
            return SemanticParser.DoParser<ModifierGroup>(item);
        }

        public double CalcValue()
        {
            double modifierValue = 0.0;
            if(modifiers != null)
            {
                modifierValue = modifiers.Where(x => x.condition == null || x.condition.Rslt()).Sum(x => x.value);
            }

            var rslt = baseValue != null ? baseValue.Value + modifierValue : modifierValue;

            return rslt > 0 ? rslt : 0;
        }

        [SemanticProperty("base")]
        double? baseValue = null;

        [SemanticPropertyArray("modifier")]
        List<Modifier> modifiers = null;
    }

    internal class Modifier
    {
        public static Modifier Parse(SyntaxItem item)
        {
            return SemanticParser.DoParser<Modifier>(item);
        }

        [SemanticProperty("value")]
        internal double value;

        [SemanticProperty("condition")]
        internal Condition condition;
    }
}