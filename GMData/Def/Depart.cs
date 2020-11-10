using System;
using System.Collections.Generic;
using System.IO;
using DataVisit;
using GMData.Mod;
using Parser.Semantic;

namespace GMData.Def
{
    public class Depart
    {

        public string key;
        public string file;

        [SemanticProperty("color")]
        public COLOR color;

        [SemanticPropertyArray("pop")]
        public List<POP_INIT> pop_init;

        public class POP_INIT
        {
            [SemanticProperty("type")]
            public string type;

            [SemanticProperty("num")]
            public double num;

            [SemanticProperty("party")]
            public string party;
        }

        public class COLOR
        {
            [SemanticProperty("r")]
            public double r;

            [SemanticProperty("g")]
            public double g;

            [SemanticProperty("b")]
            public double b;
        }

        internal static List<Depart> Load(string name, string path)
        {
            List<Depart> rslt = new List<Depart>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var depart = ModElementLoader.Load<Depart>(file, File.ReadAllText(file));
                depart.file = path;
                depart.key = Path.GetFileNameWithoutExtension(file);

                rslt.Add(depart);
            }

            return rslt;
        }
    }
}
