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
    public abstract class Text
    {
        public string Format;
        public string[] Params;

        internal Text(GroupValue groupValue, string defaultValue)
        {
            if (groupValue == null)
            {
                Format = defaultValue;
                Params = new string[] { };
                return;
            }

            Format = groupValue.First().ToString();
            Params = groupValue.Skip(1).Select(x => x.ToString()).ToArray();
        }
    }

    public class Title : Text
    {
        public Title(GroupValue groupValue, string fileName) : base(groupValue, $"{fileName}_TITLE")
        {

        }
    }

    public class Desc : Text
    {
        public Desc(GroupValue groupValue, string fileName) : base(groupValue, $"{fileName}_DESC")
        {

        }
    }
}
