using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Semantic
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SemanticPropertyArray : Attribute
    {
        public SemanticPropertyArray(string key)
        {
            this.key = key;
        }

        internal string key;
    }
}
