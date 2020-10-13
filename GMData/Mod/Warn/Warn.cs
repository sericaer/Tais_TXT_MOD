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
    public class Warn
    {
        internal string file;

        internal List<string> datas = new List<string>();

        [SemanticProperty("trigger")]
        internal Condition trigger;

        [SemanticProperty("desc")]
        internal GroupValue _desc;

        public Desc desc
        {
            get
            {
                return new Desc(_desc, key);
            }
        }

        public string key
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(file);
            }
        }

        internal bool isValid()
        {
            return trigger.Rslt();
        }
    }
}
