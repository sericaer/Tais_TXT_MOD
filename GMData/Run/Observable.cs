using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    //public class ObservableValue<T> : RValue<T>
    //{
    //    internal readonly IObservable<T> obs;
    //    private T _value;

    //    public override T Value { get { return _value; } }

    //    public ObservableValue(IObservable<T> param)
    //    {
    //        obs = param;

    //        obs.Subscribe(x => _value = x);
    //    }

    //    public IDisposable Subscribe(Action<T> action)
    //    {
    //        return obs.Subscribe(action);
    //    }
    //}

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

    [JsonConverter(typeof(SubjectValueConverter))]
    public class SubjectValue<T> : RWValue<T>, ISubject<T>
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

            if(param == null)
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
        public double value; 
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ObsBufferedValue
    {
        [JsonProperty]
        public SubjectValue<double> baseValue;

        public SourceCache<BufferInfo, string> buffers;

        public OBSValue<double> value;

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

        public ObsBufferedValue()
        {
            value = new OBSValue<double>(0);

            buffers = new SourceCache<BufferInfo, string>(x=>x.key);

            OnDeserialized(new StreamingContext());
        }

        public void SetBaseValue(double value)
        {
            if (this.baseValue == null)
            {
                this.baseValue = new SubjectValue<double>(value);

                baseValue.Subscribe(x => this.value.OnNext(CalcValue()));
            }
            else
            {
                this.baseValue.Value = value;
            }

        }

        public void SetBuffer(string key, double? value)
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

        private double CalcValue()
        {
            double rslt = 0;
            foreach (double elem in buffers.Items.Select(x=>x.value))
            {
                rslt += elem;
            }
            rslt += baseValue == null ? 0 : baseValue.Value;

            return rslt;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            buffers.Connect().Subscribe(x => value.OnNext(CalcValue()));

            if (this.baseValue != null)
            {
                baseValue.Subscribe(x => this.value.OnNext(CalcValue()));
            }
        }
    }

    internal class SourceCacheBufferConverter : JsonConverter<SourceCache<BufferInfo, string>>
    {
        //public override ReadWriteValue ReadJson(JsonReader reader, Type objectType, ReadWriteValue existingValue, bool hasExistingValue, JsonSerializer serializer)
        //{
        //    var genericArgs = objectType.GetGenericArguments();

        //    object param = reader.Value;

        //    var rslt = Activator.CreateInstance(objectType, new object[] { param.CastToReflected(genericArgs[0]) }) as ReadWriteValue;
        //    return rslt;
        //}

        //public override void WriteJson(JsonWriter writer, ReadWriteValue value, JsonSerializer serializer)
        //{
        //    writer.WriteValue(value.getValue());
        //}
        public override SourceCache<BufferInfo, string> ReadJson(JsonReader reader, Type objectType, SourceCache<BufferInfo, string> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            //var test = reader.Value;
            //var rslt = new SourceCache<BufferInfo, string>(x => x.key);
            return existingValue;

        }

        public override void WriteJson(JsonWriter writer, SourceCache<BufferInfo, string> value, JsonSerializer serializer)
        {
            //JArray date = JArray.FromObject(value.Items);
            //date.WriteTo(writer);
        }
    }
}
