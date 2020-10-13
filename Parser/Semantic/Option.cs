using Parser.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Semantic
{
    public class Option
    {
        [SemanticProperty("desc")]
        public GroupValue desc;

        [SemanticProperty("select")]
        public Select set;

        [SemanticProperty("next")]
        public Next next;

        [SemanticProperty("next_random")]
        public RandomEvents nextRandom;

        public static Option Parse(SyntaxItem item)
        {
            return SemanticParser.DoParser<Option>(item);
        }
    }
}
