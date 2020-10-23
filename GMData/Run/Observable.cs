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
    public class ObservableBufferedValue
    {
        [JsonProperty]
        public double? baseValue;

        [JsonProperty]
        public OrderedDictionary buffers = new OrderedDictionary();

        public SubjectValue<double> value = new SubjectValue<double>(0);

        public void SetBaseValue(double baseValue)
        {
            this.baseValue = baseValue;
            UpdateValue();
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


        private void UpdateValue()
        {
            double rslt = 0;
            foreach(double elem in buffers.Values)
            {
                rslt += elem;
            }
            rslt += baseValue == null ? 0 : baseValue.Value;

            value.Value = rslt;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            UpdateValue();
        }
    }

}
