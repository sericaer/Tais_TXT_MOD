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
        internal class Trigger
        {
            internal Condition raw;

            public Trigger(Condition raw)
            {
                if (raw == null)
                {
                    throw new Exception("event must have trigger");
                }

                this.raw = raw;
            }

            internal bool isTrue()
            {
                return raw.Rslt();
            }
        }
    }
}
