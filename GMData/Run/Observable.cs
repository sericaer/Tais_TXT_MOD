using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using DataVisit;
using DynamicData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GMData.Run
{
    public class OBSValue<T> : RValue<T>, ISubject<T>
    {
        internal readonly ReplaySubject<T> obs;
        bool isStart = false;
        public override T Value
        {
            get
            {
                if(!isStart)
                {
                    throw new Exception("current OBS not start!");
                }

                return obs.First();
            }
        }

        public OBSValue()
        {
            obs = new ReplaySubject<T>(1);
        }

        public OBSValue(T init)
        {
            obs = new ReplaySubject<T>(1);
            OnNext(init);
        }

        public void OnCompleted()
        {
            obs.OnCompleted();
        }

        public void OnError(Exception error)
        {
            obs.OnError(error);
        }

        public void OnNext(T value)
        {
            isStart = true;
            obs.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return obs.Subscribe(observer);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return obs.Subscribe(action);
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SubjectValue<T> : RWValue<T>, ISubject<T>
    {
        internal BehaviorSubject<T> obs;

        [JsonProperty]
        public override T Value
        {
            get
            {
                return obs.Value;
            }
            set
            {
                OnNext(value);
            }
        }

        public SubjectValue(T param)
        {
            obs = new BehaviorSubject<T>(param);
        }

        public void OnCompleted()
        {
            obs.OnCompleted();
        }

        public void OnError(Exception error)
        {
            obs.OnError(error);
        }

        public void OnNext(T value)
        {
            if(value == null)
            {
                throw new Exception();
            }

            obs.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return obs.Subscribe(observer);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return obs.Subscribe(action);
        }
    }

    public class SubjectValueConverter : JsonConverter<ReadWriteValue>
    {
        public override ReadWriteValue ReadJson(JsonReader reader, Type objectType, ReadWriteValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var genericArgs = objectType.GetGenericArguments();

            object param = reader.Value;

            if (param == null)
            {
                return null;
            }

            var rslt = Activator.CreateInstance(objectType, new object[] { param.CastToReflected(genericArgs[0]) }) as ReadWriteValue;
            return rslt;
        }


        public override void WriteJson(JsonWriter writer, ReadWriteValue value, JsonSerializer serializer)
        {
            writer.WriteValue(value.getValue());
        }
    }

    public class BufferInfo
    {
        public string key;
        public decimal value; 
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ObsBufferedValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty]
        public decimal? baseValue { get; set; }

        public SourceCache<BufferInfo, string> buffers;

        public decimal value { get; private set; }

        private Func<decimal?, IEnumerable<decimal>, decimal> CalcValue = (baseValue, buffs) =>
        {
            decimal rslt = 0;
            foreach (decimal elem in buffs)
            {
                rslt += elem;
            }
            rslt += baseValue == null ? 0 : baseValue.Value;

            return rslt;
        };

        [JsonProperty]
        private List<BufferInfo> _buffers
        {
            get
            {
                return buffers.Items.ToList();
            }
            set
            {
                buffers.AddOrUpdate(value);
            }
        }

        
        public ObsBufferedValue(Func<decimal?, IEnumerable<decimal>, decimal> CalcValue = null)
        {
            buffers = new SourceCache<BufferInfo, string>(x=>x.key);
            if(CalcValue != null)
            {
                this.CalcValue = CalcValue;
            }

            OnDeserialized(new StreamingContext());
        }

        public void SetBaseValue(decimal value)
        {
            this.baseValue = value;

        }

        public void SetBuffer(string key, decimal? value)
        {
            if (value == null)
            {
                buffers.Remove(key);
            }
            else
            {
                var newBuffer = new BufferInfo();
                newBuffer.key = key;
                newBuffer.value = value.Value;

                buffers.AddOrUpdate(newBuffer);
            }
        }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            buffers.Connect().Subscribe(x => value  = CalcValue(baseValue, buffers.Items.Select(b=>b.value)));

            this.OBSProperty(x => x.baseValue).Subscribe(_ => value = CalcValue(baseValue, buffers.Items.Select(b => b.value)));
        }
    }
}
