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

        [JsonProperty]
        public GMSourceList<string> selectedChoices;

        public IEnumerable<Def.IChoice> unselectOpts => def.options.Where(x => !selectedChoices.Items.Contains<string>(x.desc.name));

        public bool isEnd => percent >= 100;

        internal Def.IRisk def => funcGetDef(key); // GMRoot.define.risks.Single(x => x.key == key);

        public Risk(string key)
        {
            this.key = key;
            this.percent = 0.0M;
            this.selectedChoices = new GMSourceList<string>();
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

            if(selectedChoices.Count != 0)
            {
                var selected = def.options.Where(x => selectedChoices.Items.Contains<string>(x.desc.name));
                foreach (var elem in selected)
                {
                    var randomEvent = elem.CalcRandomEvent(this);
                    if (randomEvent != null)
                    {
                        yield return randomEvent;
                    }
                }
            }

        }

        public void SelectChoice(int index)
        {
            var optName = def.options[index].desc.name;
            if (selectedChoices.Items.Contains<string>(optName))
            {
                return;
            }

            selectedChoices.Add(optName);
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GMSourceList<T> : ISourceList<T>
    {
        [JsonProperty]
        public IEnumerable<T> elems
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

        public GMSourceList()
        {
            list = new SourceList<T>();
        }

        private SourceList<T> list;

        public IObservable<int> CountChanged => list.CountChanged;

        public IEnumerable<T> Items => list.Items;

        public int Count => list.Count;

        public IObservable<IChangeSet<T>> Connect(Func<T, bool> predicate = null)
        {
            return list.Connect(predicate);
        }

        public void Dispose()
        {
            list.Dispose();
        }

        public void Edit(Action<IExtendedList<T>> updateAction)
        {
            list.Edit(updateAction);
        }

        public IObservable<IChangeSet<T>> Preview(Func<T, bool> predicate = null)
        {
            return list.Preview(predicate);
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Risks : GMSourceList<Risk>
    {
        [DataVisitorProperty("start")]
        public string start
        {
            set
            {
                this.Add(new Risk(value));
            }
        }

        internal IEnumerable<GMData.Mod.IGEvent> DaysInc()
        {
            foreach(var risk in Items)
            {
                foreach ( var eventObj in risk.DaysInc())
                {
                    yield return eventObj;
                }

                if (risk.isEnd)
                {
                    this.Remove(risk);
                }
            }
        }

        public Risks()
        {

        }

    }
}