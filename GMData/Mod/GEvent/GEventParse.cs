using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;
using Parser.Semantic;
using Parser.Syntax;

namespace GMData.Mod
{

    public partial class GEvent
    {
        public class GEventParse
        {
            [SemanticPropertyArray("option")]
            public List<Parser.Semantic.Option> options;

            [SemanticProperty("trigger")]
            public Condition trigger;

            [SemanticProperty("occur")]
            internal int? occur;

            [SemanticProperty("title")]
            internal GroupValue title;

            [SemanticProperty("desc")]
            internal GroupValue desc;

            [SemanticProperty("date")]
            internal Parser.Semantic.Date date;
        }
    }

}
