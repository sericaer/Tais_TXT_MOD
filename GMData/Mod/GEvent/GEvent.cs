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
    public interface IGEvent
    {
        string key { get;}

        Title title { get; }
        Desc desc { get; }
        Option[] options { get;}

        Tuple<string, object> objTuple { get; set; }

        Func<string, IGEvent> GetNext { get; set; }
    }

    public abstract partial class GEvent : IGEvent
    {
        public string key { get => Path.GetFileNameWithoutExtension(file);}

        public Title title { get => _title; }
        public Desc desc { get => _desc; }
        public Option[] options { get => _options; }

        public Tuple<string, object> objTuple { get => _objTuple; set => _objTuple = value; }

        public Func<string, IGEvent> GetNext { get; set; }

        internal string file;

        internal GEventParse parse;

        private Title _title;
        private Desc _desc;
        private Option[] _options;
        private Tuple<string, object> _objTuple;

        internal static Dictionary<string, GEvent> Load(string name, string path)
        {
            var dict = new Dictionary<string, GEvent>();

            GEventCommon.Load(name, path + "/common", ref dict);

            return dict;
        }

        internal abstract IEnumerable<GEvent> Check();

        public bool isValid()
        {
            if(parse.date != null && !parse.date.isTrue())
            {
                return false;
            }

            if(!parse.trigger.Rslt())
            {
                return false;
            }

            if(!Tools.GRandom.isOccur(parse.occur.Value))
            {
                return false;
            }

            return true;
        }

        public GEvent(string file)
        {
            this.file = file;
            this.parse = ModElementLoader.Load<GEventParse>(file, File.ReadAllText(file));

            this._title = new Title(parse.title, key);
            this._desc = new Desc(parse.desc, key);
            this._options = parse.options.Select((v, i) => new Option { semantic = v, index = i + 1, ownerName = Path.GetFileNameWithoutExtension(file) }).ToArray();


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
        
        
        
    }

}
