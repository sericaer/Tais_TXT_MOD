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
        public class Date
        {
            internal Date(Parser.Semantic.Date raw)
            {
                this.raw = raw;
            }

            internal bool isEqual((int year, int month, int day) date)
            {
                if (raw == null)
                {
                    return true;
                }

                if (raw.year != null && raw.year != date.year)
                {
                    return false;
                }
                if (raw.month != null && raw.month != date.month)
                {
                    return false;
                }
                if (raw.day != null && raw.day != date.day)
                {
                    return false;
                }

                return true;
            }

            Parser.Semantic.Date raw;
        }
    }

}
