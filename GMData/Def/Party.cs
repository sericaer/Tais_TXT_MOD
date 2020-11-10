using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GMData.Mod;
using Parser.Semantic;

namespace GMData.Def
{
    public class Party
    {
        public string key;
        public string file;

        [SemanticPropertyArray("relation")]
        public List<Relation> relation;

        internal static List<Party> Load(string modname, string path)
        {
            List<Party> rslt = new List<Party>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var party = ModElementLoader.Load<Party>(file, File.ReadAllText(file));
                party.file = path;
                party.key = Path.GetFileNameWithoutExtension(file);

                rslt.Add(party);
            }

            return rslt;
        }

        public class Relation
        {
            [SemanticProperty("peer")]
            public string peer;

            [SemanticProperty("value")]
            public double value;
        }
    }
}
