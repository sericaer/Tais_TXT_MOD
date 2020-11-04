using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    public class ObservableValue<T> : RValue<T>
    {
        internal readonly IObservable<T> obs;
        private T _value;

        public override T Value { get { return _value; } }

        public ObservableValue(IObservable<T> param)
        {
            obs = param;

            obs.Subscribe(x => _value = x);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return obs.Subscribe(action);
        }
    }

    public class ObservableValueEx<T> : RValue<T>, ISubject<T>
    {
        internal readonly BehaviorSubject<T> obs;

        public override T Value { get { return obs.First(); } }

        public ObservableValueEx()
        {
            obs = new BehaviorSubject<T>(default(T));
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

    [JsonConverter(typeof(SubjectValueConverter))]
    public class SubjectValue<T> : RWValue<T>
    {
        internal readonly BehaviorSubject<T> obs;

        public override T Value
        {
            get
            {
                return obs.Value;
            }
            set
            {
                obs.OnNext(value);
            }
        }

        public SubjectValue(T param)
        {
            obs = new BehaviorSubject<T>(param);
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

            var rslt = Activator.CreateInstance(objectType, new object[] { param.CastToReflected(genericArgs[0]) }) as ReadWriteValue;
            return rslt;
        }

        public override void WriteJson(JsonWriter writer, ReadWriteValue value, JsonSerializer serializer)
        {
            writer.WriteValue(value.getValue());
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ObsBufferedValue
    {
        public SubjectValue<double> baseValue;
        public ObservableValueEx<double> value;

        [JsonProperty]
        public OrderedDictionary buffers;

        public ObsBufferedValue()
        {
            value = new ObservableValueEx<double>();

            buffers = new OrderedDictionary();
        }

        public void SetBaseValue(double value)
        {
            if(this.baseValue == null)
            {
                this.baseValue = new SubjectValue<double>(value);
            }
            else
            {
                this.baseValue.Value = value;
            }

            this.baseValue.Subscribe(x => UpdateValue());
        }

        public void SetBuffer(string key, double value)
        {
            if(buffers.Contains(key))
            {
                buffers[key] = value;
            }
            else
            {
                buffers.Add(key, value);
            }
            
            UpdateValue();
        }

        public void SetBuffer(string key, double? value)
        {
            if(value == null)
            {
                buffers.Remove(key);
            }
            else
            {
                if (buffers.Contains(key))
                {
                    buffers[key] = value.Value;
                }
                else
                {
                    buffers.Add(key, value.Value);
                }
            }

            UpdateValue();
        }

        private void UpdateValue()
        {
            double rslt = 0;
            foreach(double elem in buffers.Values)
            {
                rslt += elem;
            }
            rslt += baseValue == null ? 0 : baseValue.Value;

            value.OnNext(rslt);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            UpdateValue();
        }
    }

}
