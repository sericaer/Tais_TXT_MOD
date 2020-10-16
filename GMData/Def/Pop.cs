using System;
using System.Collections.Generic;
using System.IO;
using GMData.Mod;
using Parser.Semantic;

namespace GMData.Def
{
    public class Pop
    {
        public string key;
        public string file;

        [SemanticProperty("is_collect_tax")]
        public bool is_collect_tax;

        [SemanticProperty("consume")]
        public double? consume;

        internal static List<Pop> Load(string name, string path)
        {
            List<Pop> rslt = new List<Pop>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var Pop = ModElementLoader.Load<Pop>(file, File.ReadAllText(file));
                Pop.file = path;
                Pop.key = Path.GetFileNameWithoutExtension(file);

                rslt.Add(Pop);
            }

            return rslt;
        }
    }
}
