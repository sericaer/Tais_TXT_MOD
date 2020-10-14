using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parser.Semantic;

namespace GMData.Def
{
    public class Party
    {
        public string key;
        public string file;


        public Party(string file)
        {
            this.file = file;
            this.key = Path.GetFileNameWithoutExtension(file);
        }

        internal static List<Party> Load(string modname, string path)
        {
            List<Party> rslt = new List<Party>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                rslt.Add(new Party(file));
            }

            return rslt;
        }
    }
}
