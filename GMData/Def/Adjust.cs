using System;
using System.Collections.Generic;
using System.IO;
using GMData.Mod;
using Parser.Semantic;

namespace GMData.Def
{
    public class Adjust
    {
        public string key;

        [SemanticProperty("init")]
        public Init init;

        [SemanticPropertyArray("level")]
        public List<Level> levels;

        private string file;

        public class Level
        {
            [SemanticProperty("percent")]
            internal double percent;

            [SemanticProperty("effect_pop_consume")]
            internal double? effect_pop_consume;
        }

        internal static List<Adjust> Load(string name, string path)
        {
            var rslt = new List<Adjust>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var adjust = ModElementLoader.Load<Adjust>(file, File.ReadAllText(file));
                adjust.file = file;
                adjust.key = Path.GetFileNameWithoutExtension(file);
                rslt.Add(adjust);
            }

            return rslt;
        }

        public class Init
        {
            [SemanticProperty("level")]
            public int level;

            [SemanticProperty("valid")]
            public bool valid;
        }
    }
}
