using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
        public ObservableValue<double> baseValue;

        [JsonProperty]
        public ObservableCollection<(string name, double value)> buffers;

        public ObservableValue<double> value;


        internal ObservableBufferedValue(IObservable<double> baseValue)
        {
            this.baseValue = new ObservableValue<double>(baseValue);
            this.buffers = new ObservableCollection<(string name, double value)>();

            var bufferChanged = Observable.FromEventPattern(
                                    (EventHandler<NotifyCollectionChangedEventArgs> ev)
                                        => new NotifyCollectionChangedEventHandler(ev),
                                           ev => buffers.CollectionChanged += ev,
                                           ev => buffers.CollectionChanged -= ev)
                                .Select(x => buffers.Sum(y => y.value))
                                .StartWith(buffers.Sum(y => y.value));

            value = Observable.CombineLatest(this.baseValue.obs, bufferChanged, (x, y) => x + y).ToOBSValue();
        }


        [JsonConstructor]
        private ObservableBufferedValue()
        {
            this.buffers = new ObservableCollection<(string name, double value)>();
        }

    }

}
