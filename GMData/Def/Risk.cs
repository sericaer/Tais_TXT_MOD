using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GMData.Mod;
using Parser.Semantic;

namespace GMData.Def
{
    internal interface IRisk
    {
        string key { get; set; }

        decimal cost_days { get; set; }

        string endEvent { get; set; }

        RandomEvents randomEvent { get; set; }

        List<Parser.Semantic.Option> options { get; set; }

        Func<Run.Risk, IGEvent> CalcEndEvent { get; }

        Func<Run.Risk, IGEvent> CalcRandomEvent { get;  }
    }

    internal class Risk : IRisk
    {
        private string file;

        public string key { get; set; }

        [SemanticProperty("cost_days")]
        public decimal cost_days { get; set; }

        [SemanticProperty("end_event")]
        public string endEvent { get; set; }

        [SemanticProperty("random_event")]
        public RandomEvents randomEvent { get; set; }

        [SemanticPropertyArray("option")]
        public List<Parser.Semantic.Option> options { get; set; }
        Func<Run.Risk, IGEvent> IRisk.CalcEndEvent { get => CalcEndEvent; }
        Func<Run.Risk, IGEvent> IRisk.CalcRandomEvent { get => CalcRandomEvent; }


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

        public IGEvent CalcEndEvent(Run.Risk risk)
        {
            if (endEvent != null)
            {
                return null;
            }

            var eventObj = GMRoot.modder.FindEvent(endEvent);
            eventObj.objTuple = new Tuple<string, object>("risk", risk);
            return eventObj;
        }

        internal IEnumerable<(string name, decimal value)> CalcRandomEventGroup(Run.Risk risk)
        {
            Visitor.SetCurrObj("risk", risk);

            var randomGroup = randomEvent?.Calc().Where(x => x.value > 0);

            Visitor.RemoveCurrObj();

            return randomGroup;
        }

        public IGEvent CalcRandomEvent(Run.Risk risk)
        {
            Visitor.SetCurrObj("risk", risk);

            var randomGroup = randomEvent?.Calc().Where(x => x.value > 0);

            Visitor.RemoveCurrObj();

            if (randomGroup != null)
            {
                var randomEvent = Tools.GRandom.CalcGroup(randomGroup);
                var eventObj = GMRoot.modder.FindEvent(randomEvent);
                eventObj.objTuple = new Tuple<string, object>("risk", risk);
                return eventObj;
            }

            return null;
        }

        //public string CalcRandomEvent()
        //{
        //    return randomEvent.Calc();
        //}

    }
}
