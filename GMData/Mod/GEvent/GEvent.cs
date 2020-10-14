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
        public string key;
        public Title title;
        public Desc desc;
        public Option[] options;

        internal string file;

        internal GEventParse parse;

        public bool isValid()
        {
            if(parse.date != null || !parse.date.isTrue())
            {
                return false;
            }

            if(!parse.trigger.Rslt())
            {
                return false;
            }

            if(!Tools.GRandom.isOccur(100 / parse.occur.Value))
            {
                return false;
            }

            return true;
        }

        public GEvent(string file)
        {
            this.file = file;
            this.parse = ModElementLoader.Load<GEventParse>(file, File.ReadAllText(file));

            this.key = Path.GetFileNameWithoutExtension(file);

            this.title = new Title(parse.title, key);
            this.desc = new Desc(parse.desc, key);
            this.options = parse.options.Select((v, i) => new Option { semantic = v, index = i + 1, ownerName = Path.GetFileNameWithoutExtension(file) }).ToArray();


            if (this.parse.trigger is ConditionDefault && !this.parse.trigger.Rslt())
            {

            }
            else
            {
                if (this.parse.occur == null)
                {
                    throw new Exception("event must have occur when trigger is not default false");
                }
            }
        }

        public Func<string, GEvent> GetNext;
    }

}
