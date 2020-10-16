using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Parser.Syntax;

namespace Parser.Semantic
{
    public class SemanticParser
    {
        public static T ParserFile<T>(string path)
        {
            try
            {
                var syntaxRoot = SyntaxItem.RootParse(File.ReadAllText(path));

                return DoParser<T>(syntaxRoot);
            }
            catch (Exception e)
            {
                throw new Exception($"Parse error in script:{path}", e);
            }
        }

        public static T DoParser<T>(SyntaxItem syntaxRoot)
        {
            return (T)DoParser(syntaxRoot, typeof(T));
        }

        public static object DoParser(SyntaxItem syntaxRoot, Type t)
        {
            object rslt = Activator.CreateInstance(t);

            var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var Properties = field.GetCustomAttributes(typeof(SemanticProperty), false);
                if (Properties.Count() != 0)
                {
                    ParseSemanticProperty(syntaxRoot, Properties.First() as SemanticProperty, field, ref rslt);
                    continue;
                }

                Properties = field.GetCustomAttributes(typeof(SemanticPropertyArray), false);
                if (Properties.Count() != 0)
                {
                    ParseSemanticPropertyList(syntaxRoot, Properties.First() as SemanticPropertyArray, field, ref rslt);
                }
            }

            return rslt;
        }

        private static void ParseSemanticProperty(SyntaxItem syntaxRoot, SemanticProperty property, FieldInfo field, ref object obj)
        {
            var item = syntaxRoot.Find(property.key);
            if (item == null)
            {
                return;
                //throw new Exception($"can not find key:{property.key}");
            }

            field.SetValue(obj, ConvertItem(item, field.FieldType));
        }

        private static void ParseSemanticPropertyList(SyntaxItem syntaxRoot, SemanticPropertyArray property, FieldInfo field, ref object obj)
        {
            var items = syntaxRoot.Finds(property.key);
            if (!items.Any())
            {
                return;
                //throw new Exception($"can not find key:{property.key}");
            }


            if (!field.FieldType.IsGenericType || field.FieldType.GetGenericTypeDefinition() != typeof(List<>))
            {
                throw new Exception($"field type not list! {field.FieldType.FullName}");
            }

            dynamic list = Activator.CreateInstance(field.FieldType);
            
            Type[] listParameters = field.FieldType.GetGenericArguments();

            foreach (var item in items)
            {
                list.Add((dynamic)ConvertItem(item, listParameters[0]));
            }

            field.SetValue(obj, list);
        }

        private static object ConvertItem(SyntaxItem item, Type type)
        {
            Type currType = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type[] listParameters = type.GetGenericArguments();
                if(listParameters[0].IsValueType)
                {
                    currType = listParameters[0];
                }
            }

            if(currType.IsValueType || currType == typeof(string))
            {
                if (item.values.Count() != 1 || !(item.values[0] is SingleValue))
                {
                    throw new Exception($"type {type.FullName} not support key-value:{item}");
                }

                var strValue = item.values[0] as SingleValue;
                if (currType == typeof(string))
                {
                    return strValue.ToString();
                }
                else if (currType == typeof(double) || currType == typeof(int))
                {
                    var digitValue = strValue as DigitValue;
                    if (digitValue == null)
                    {
                        throw new Exception($"can not support type {type} with key:{item.key}");
                    }

                    return Convert.ChangeType(digitValue.digit, currType);
                }
                else if (currType == typeof(bool))
                {
                    var bValue = strValue as BoolValue;
                    if (bValue == null)
                    {
                        throw new Exception($"can not support type {type} with key:{item.key}");
                    }

                    return Convert.ChangeType(bValue.data, currType);
                }
                else
                {
                    throw new Exception($"can not support type {currType} with key:{item.key}");
                }
            }
            else
            {
                var ParseMethod = currType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
                if (ParseMethod != null)
                {
                    return ParseMethod.Invoke(null, new object[] { item });
                }

                return DoParser(item, type);
            }

        }
    }
}
