using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GMData;
using GMData.Run;

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

        public static ObservableValue<T> ToOBSValue<T>(this IObservable<T> obs)
        {
            return new ObservableValue<T>(obs);
        }

        public static void DaysInc(this List<Risk> list)
        {
            list.RemoveAll(x => x.isEnd);
            list.ForEach(x => x.DaysInc());
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
}
