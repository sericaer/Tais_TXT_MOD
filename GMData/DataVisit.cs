using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DataVisit
{
    public class Visitor
    {
        static Dictionary<Type, Dictionary<string, ReflectionInfo>> dictRef = new Dictionary<Type, Dictionary<string, ReflectionInfo>>();
        public class Pos
        {
            internal int index;
            internal IList list;
        }

        public static object Get(string raw)
        {
            //int intRslt;
            //if (int.TryParse(raw, out intRslt))
            //{
            //    return intRslt;
            //}

            //double doubleRslt;
            //if (double.TryParse(raw, out doubleRslt))
            //{
            //    return doubleRslt;
            //}

            if (!raw.Contains("."))
            {
                return raw;
            }

            //var splits = raw.Split('.');
            //if (splits.Length < 2)
            //{
            //    throw new Exception("error value " + raw);
            //}

            return VisitGet(raw);
        }

        internal static void SetCurr(object obj)
        {
            currObj = obj;
        }

        //public static object Get(string raw, Pos pos)
        //{
        //    object rslt = pos.obj;
        //    foreach (var field in dictMap[raw])
        //    {
        //        rslt = field.GetValue(rslt);
        //    }

        //    return rslt;
        //}

        public static void Set(string raw, object value)
        {
            var splits = raw.Split('.');

            var type = rootObj.GetType();
            var obj = rootObj;

            int i = 0;
            if (enumerateObj != null && enumerateKey == splits[0])
            {
                type = enumerateObj.GetType();
                obj = enumerateObj;
                i++;
            }

            for (; i<splits.Length-1; i++)
            {
                var reflection = dictRef[type][splits[i]];
                obj = reflection.GetValue(obj);
                type = reflection.GetDataType();
            }

            var leaf = dictRef[type][splits[i]];

            if (leaf.GetDataType().IsSubclassOf(typeof(ReadValue)))
            {
                throw new Exception($"key:{raw} only read!");
            }

            if(leaf.GetDataType().IsSubclassOf(typeof(ReadWriteValue)))
            {
                var leafObj = leaf.GetValue(obj) as ReadWriteValue;
                leafObj.setValue(value);
                return;
            }

            leaf.SetValue(obj, value);
        }

        public static void SetVisitData(object data)
        {
            rootObj = data;
        }

        public static void InitVisitMap(Type type)
        {
            if(dictRef.ContainsKey(type))
            {
                return;
            }

            var dictField = AnaylizeFields(type);
            var dictProperty = AnaylizeProperties(type);

            dictRef.Add(type, new Dictionary<string, ReflectionInfo>());

            dictField.ToList().ForEach(x => dictRef[type].Add(x.Key, x.Value));
            dictProperty.ToList().ForEach(x => dictRef[type].Add(x.Key, x.Value));

            foreach(var reflection in dictRef[type].Values)
            {
                var subType = reflection.GetDataType();
                if (subType.IsValueType)
                {
                    continue;
                }

                if (subType.IsGenericType && subType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    subType = subType.GetGenericArguments()[0];
                }

                InitVisitMap(subType);
            }
        }

        public static bool EnumerateVisit(string key, ref Pos pos)
        {
            if (pos == null)
            {
                var objs = VisitGet(key) as IList;
                if (objs == null)
                {
                    throw new Exception($"EnumerateVisit error, {key} must be List<>");
                }

                pos = new Pos() { index = 0, list = objs };
                enumerateKey = key;
            }
            else
            {
                pos.index++;
            }

            if (pos.index + 1 > pos.list.Count)
            {
                enumerateObj = null;
                enumerateKey = null;
                return false;
            }

            enumerateObj = pos.list[pos.index];

            return true;
        }

        internal static Dictionary<string, ReflectionInfo> AnaylizeFields(Type type)
        {
            var rslt = new Dictionary<string, ReflectionInfo>();

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var visitPropery = (DataVisitorProperty)Attribute.GetCustomAttribute(field, typeof(DataVisitorProperty));
                if (visitPropery != null)
                {
                    rslt.Add(visitPropery.key, new FieldReflectionInfo(field));
                }

                var visitProperyArray = (DataVisitorPropertyArray)Attribute.GetCustomAttribute(field, typeof(DataVisitorPropertyArray));
                if (visitProperyArray != null)
                {
                    if(field.FieldType.GetGenericTypeDefinition() != typeof(List<>))
                    {
                        throw new Exception();
                    }

                    rslt.Add(visitProperyArray.key, new FieldReflectionInfo(field));
                }
            }

            return rslt;
        }

        internal static Dictionary<string, ReflectionInfo> AnaylizeProperties(Type type)
        {
            var rslt = new Dictionary<string, ReflectionInfo>();

            var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var visitPropery = (DataVisitorProperty)Attribute.GetCustomAttribute(property, typeof(DataVisitorProperty));
                if (visitPropery == null)
                {
                    continue;
                }

                rslt.Add(visitPropery.key, new PropertyReflectionInfo(property));
            }

            return rslt;
        }

        private static object VisitGet(string raw)
        {
            var splits = raw.Split('.');

            var type = rootObj.GetType();
            var obj = rootObj;

            int i = 0;
            if (enumerateObj != null && enumerateKey == splits[0])
            {
                type = enumerateObj.GetType();
                obj = enumerateObj;
                i++;
            }

            for (; i < splits.Length; i++)
            {
                var reflection = dictRef[type][splits[i]];
                obj = reflection.GetValue(obj);
                type = reflection.GetDataType();
            }

            if (typeof(ReadValue).IsInstanceOfType(obj))
            {
                var rValue = obj as ReadValue;
                return rValue.getValue();
            }

            if (typeof(ReadWriteValue).IsInstanceOfType(obj))
            {
                var rValue = obj as ReadWriteValue;
                return rValue.getValue();
            }

            return obj;
        }

        private static object rootObj;
        private static object currObj;

        private static object enumerateObj;
        private static string enumerateKey;
    }

    internal abstract class ReflectionInfo
    {
        internal string name;

        internal abstract object GetValue(object rslt);

        internal abstract void SetValue(object rslt, object value);

        internal abstract Type GetDataType();

        internal abstract Type GetDeclaringType();

        internal abstract ReflectionInfo GetChild(string name);

        internal abstract (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj);
    }

    internal class FieldReflectionInfo : ReflectionInfo
    {
        internal FieldReflectionInfo(string name, FieldInfo field)
        {
            var fields = Visitor.AnaylizeFields(field.FieldType);
            var propertys = Visitor.AnaylizeProperties(field.FieldType);

            this.name = name;
            this.field = field;
        }

        internal FieldReflectionInfo(FieldInfo field)
        {
            this.field = field;
        }

        internal override object GetValue(object obj)
        {
            return field.GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            field.SetValue(obj, value);
        }

        internal override Type GetDataType()
        {
            return field.FieldType;
        }

        internal override Type GetDeclaringType()
        {
            return field.DeclaringType;
        }

        internal override ReflectionInfo GetChild(string name)
        {
            throw new NotImplementedException();
        }

        internal override (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj)
        {
            throw new NotImplementedException();
        }

        private FieldInfo field;

    }

    internal class PropertyReflectionInfo : ReflectionInfo
    {
        internal PropertyReflectionInfo(string name, PropertyInfo field)
        {
            this.name = name;
            this.property = field;
        }


        internal PropertyReflectionInfo(PropertyInfo property)
        {
            this.property = property;
        }

        internal override object GetValue(object obj)
        {
            return property.GetValue(obj);
        }

        internal override void SetValue(object obj, object value)
        {
            property.SetValue(obj, value);
        }

        internal override Type GetDataType()
        {
            return property.PropertyType;
        }

        internal override Type GetDeclaringType()
        {
            return property.DeclaringType;
        }

        internal override (ReflectionInfo reflectionInfo, object obj) GetChild(string name, object currObj)
        {
            var obj = GetValue(currObj);
            return (null, obj);
        }

        internal override ReflectionInfo GetChild(string name)
        {
            throw new NotImplementedException();
        }

        internal PropertyInfo property;
    }

    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class DataVisitorProperty : Attribute
    {
        public DataVisitorProperty(string key)
        {
            this.key = key;
        }

        public DataVisitorProperty()
        {
            this.key = "";
        }

        internal string key;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataVisitorPropertyArray : Attribute
    {
        public DataVisitorPropertyArray(string key)
        {
            this.key = key;
        }

        public DataVisitorPropertyArray()
        {
            this.key = "";
        }

        internal string key;
    }

    public abstract class ReadValue
    {
        public abstract object getValue();
    }

    public abstract class ReadWriteValue
    {
        public abstract object getValue();
        public abstract void   setValue(object value);
    }

    public class RValue<T> : ReadValue
    {
        public virtual T Value { get; }

        public override object getValue()
        {
            return Value;
        }
    }

    public class RWValue<T> : ReadWriteValue
    {
        public virtual T Value { get; set; }

        public override object getValue()
        {
            return Value;
        }

        public override void setValue(object value)
        {
            Value = (T)value;
        }
    }
}
