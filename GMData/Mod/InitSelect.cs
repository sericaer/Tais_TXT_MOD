using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parser.Semantic;

namespace GMData.Mod
{
    public class InitSelect
    {
        public string key;
        public string file;

        public bool isFirst;
        public Desc desc;
        public Option[] options;

        public InitSelect(string file)
        {
            this.file = file;
            this.key = Path.GetFileNameWithoutExtension(file);

            var sematic = ModElementLoader.Load<InitSelectSematic>(file, File.ReadAllText(file));
            isFirst = sematic.isFirst != null && sematic.isFirst.Value;

            desc = new Desc(null, Path.GetFileNameWithoutExtension(file));
            options = sematic.options.Select((v, i) => new Option { semantic = v, index = i + 1, ownerName = Path.GetFileNameWithoutExtension(file) }).ToArray();
        }

        internal static List<InitSelect> Load(string modname, string path)
        {
            List<InitSelect> rslt = new List<InitSelect>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                rslt.Add(new InitSelect(file));
            }

            return rslt;
        }
    }

    public class InitSelectSematic
    {
        [SemanticProperty("is_first")]
        public bool? isFirst;

        [SemanticPropertyArray("option")]
        public List<Parser.Semantic.Option> options;
    }
}
