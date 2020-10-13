using System;
namespace Parser.Semantic
{
    public static class Visitor
    {
        public static dynamic GetValue(Syntax.Value raw)
        {
            switch(raw)
            {
                case Syntax.BoolValue bValue:
                    return bValue.data;
                case Syntax.DigitValue dValue:
                    return dValue.digit;
                case Syntax.StringValue sValue:
                    return GetValueFunc(sValue.ToString());
                default:
                    throw new NotImplementedException();
            }
        }

        public static void SetValue(string target, Syntax.Value raw)
        {
            SetValueFunc(target, GetValue(raw));
        }

        public static void SetValue(string target, object raw)
        {
            SetValueFunc(target, raw);
        }

        public static Func<String, dynamic> GetValueFunc;

        public static Action<String, object> SetValueFunc;
    }
}
