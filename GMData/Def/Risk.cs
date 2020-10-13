using System;
using System.Collections.Generic;
using System.IO;
using Parser.Semantic;

namespace GMData.Def
{
    public class Risk
    {
        [SemanticProperty("cost_days")]
        public double cost_days;

        [SemanticProperty("end_event")]
        public string endEvent;

        [SemanticProperty("random_event")]
        public RandomEvents randomEvent;

        private string file;

        public string key;

        internal static List<Risk> Load(string path)
        {
            List<Risk> rslt = new List<Risk>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            //foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            //{
            //    var risk = DefElementLoader.Load<Risk>(file, File.ReadAllText(file));
            //    risk.file = file;
            //    risk.key = Path.GetFileNameWithoutExtension(file);
            //    rslt.Add(risk);
            //}

            return rslt;
        }

        //public string CalcRandomEvent()
        //{
        //    return randomEvent.Calc();
        //}

    }
}
