using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GMData.Mod;
using Parser.Semantic;
using Parser.Syntax;

namespace GMData.Def
{
    public interface IRisk
    {
        string key { get;}

        decimal cost_days { get;}

        string endEvent { get;}

        IChoice[] options { get;}

        Func<Run.Risk, IGEvent> CalcEndEvent { get; }
    }

    internal class Risk : IRisk
    {
        private string file;

        public string key { get; set; }

        public IChoice[] options => _options.Select(x=>x as IChoice).ToArray();

        [SemanticProperty("cost_days")]
        public decimal cost_days { get; set; }

        [SemanticProperty("end_event")]
        public string endEvent { get; set; }

        [SemanticPropertyArray("choice")]
        public List<Choice> _options { get; set; }

        Func<Run.Risk, IGEvent> IRisk.CalcEndEvent { get => CalcEndEvent; }

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

        public IGEvent CalcEndEvent(object risk)
        {
            if (endEvent != null)
            {
                return null;
            }

            var eventObj = GMRoot.modder.FindEvent(endEvent);
            eventObj.objTuple = new Tuple<string, object>("risk", risk);
            return eventObj;
        }

    }
}
