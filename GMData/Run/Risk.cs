using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DataVisit;
using DynamicData;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Risk : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        internal static Func<string, Def.IRisk> funcGetDef;

        [JsonProperty]
        public string key;

        [JsonProperty]
        public decimal percent { get; set; }

        public bool isEnd => percent >= 100;

        private Def.IRisk def => funcGetDef(key); // GMRoot.define.risks.Single(x => x.key == key);

        public Risk(string key)
        {
            this.key = key;
            this.percent = 0.0M;
        }

        public IEnumerable<GMData.Mod.IGEvent> DaysInc()
        {
            percent += 100 / (decimal)def.cost_days;

            if(isEnd)
            {
                var endEvent = def.CalcEndEvent(this);
                if(endEvent != null)
                {
                    yield return endEvent;
                }
            }

            var randomEvent = def.CalcRandomEvent(this);
            if (randomEvent != null)
            {
                yield return randomEvent;
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Risks : ISourceList<Risk>
    {
        [DataVisitorProperty("start")]
        public string start
        {
            set
            {
                list.Add(new Risk(value));
            }
        }

        [JsonProperty]
        public IEnumerable<Risk> elems
        {  
            get
            {
                return list.Items;
            }
            set
            {
                list.Edit(inner =>
                {
                    inner.Clear();
                    inner.AddRange(value);
                });
            }
        }

        internal IEnumerable<GMData.Mod.IGEvent> DaysInc()
        {
            foreach(var risk in list.Items)
            {
                foreach ( var eventObj in risk.DaysInc())
                {
                    yield return eventObj;
                }

                if (risk.isEnd)
                {
                    list.Remove(risk);
                }
            }
        }

        public Risks()
        {
            list = new SourceList<Risk>();
        }

        private SourceList<Risk> list;

        public IObservable<int> CountChanged => list.CountChanged;

        public IEnumerable<Risk> Items => list.Items;

        public int Count => list.Count;

        public IObservable<IChangeSet<Risk>> Connect(Func<Risk, bool> predicate = null)
        {
            return list.Connect(predicate);
        }

        public void Dispose()
        {
            list.Dispose();
        }

        public void Edit(Action<IExtendedList<Risk>> updateAction)
        {
            list.Edit(updateAction);
        }

        public IObservable<IChangeSet<Risk>> Preview(Func<Risk, bool> predicate = null)
        {
            return list.Preview(predicate);
        }


    }
}