﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GMData.Mod;
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

        [SemanticPropertyArray("option")]
        public List<Parser.Semantic.Option> options;

        private string file;

        public string key;

        internal static List<Risk> Load(string mod, string path)
        {
            List<Risk> rslt = new List<Risk>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var risk = ModElementLoader.Load<Risk>(file, File.ReadAllText(file));
                risk.file = file;
                risk.key = Path.GetFileNameWithoutExtension(file);
                rslt.Add(risk);
            }

            return rslt;
        }

        internal IEnumerable<(string name, double value)> CalcRandomEvent(object obj)
        {
            Visitor.SetCurrObj("risk", obj);

            var randomGroup = randomEvent?.Calc().Where(x => x.value > 0);

            Visitor.RemoveCurrObj();

            return randomGroup;
        }

        //public string CalcRandomEvent()
        //{
        //    return randomEvent.Calc();
        //}

    }
}
