using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reactive.Linq;
using GMData;
using GMData.Run;
using System.Linq.Expressions;
using System.ComponentModel;
using ReactiveMarbles.PropertyChanged;

namespace GMData
{
    public static class Extensions
    {
        //public static double? calcExpectTax(this Pop pop)
        //{
        //    if(!pop.def.is_collect_tax)
        //    {
        //        return null;
        //    }

        //    return pop.num.value * 0.001;
        //    //return calcExpectTax(pop, Economy.inst.popTaxLevel);
        //}

        //public static ObservableValue<T> ToOBSValue<T>(this IObservable<T> obs)
        //{
        //    return new ObservableValue<T>(obs);
        //}

        public static void DaysInc(this List<Risk> list)
        {
            list.RemoveAll(x => x.isEnd);
            list.ForEach(x => x.DaysInc());
        }

        public static IEnumerable<TResult> SelectNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).Where(x=>x!= null);
        }

        public static IObservable<decimal> CombineLatestSum<TSource>(this IEnumerable<TSource> source, Func<TSource, IObservable<decimal>> selector)
        {
            return source.SelectNotNull(selector).CombineLatest(x => x.Sum());
        }
        public static IObservable<int> CombineLatestSum<TSource>(this IEnumerable<TSource> source, Func<TSource, IObservable<int>> selector)
        {
            return source.SelectNotNull(selector).CombineLatest(x => x.Sum());
        }

        public static bool HasAttribute<T>(this Enum value) where T : Attribute
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            T attribute = Attribute.GetCustomAttribute(field, typeof(T)) as T;

            return attribute != null;
        }

        public static IObservable<TReturn> OBSProperty<TObj, TReturn>(this TObj objectToMonitor,
            Expression<Func<TObj, TReturn>> propertyExpression)
            where TObj : class, INotifyPropertyChanged
        {
            return objectToMonitor.WhenPropertyValueChanges(propertyExpression);
        }
    }

    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object o)
        {
            dynamic d = o;
            return (T)d;
        }

        public static dynamic CastToReflected(this object o, Type type)
        {
            var methodInfo = typeof(ObjectExtensions).GetMethod(nameof(CastTo), BindingFlags.Static | BindingFlags.Public);
            var genericArguments = new[] { type };
            var genericMethodInfo = methodInfo?.MakeGenericMethod(genericArguments);
            return genericMethodInfo?.Invoke(null, new[] { o });
        }
    }

    public static class EnumEx
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).OfType<T>();
        }
    }
}
