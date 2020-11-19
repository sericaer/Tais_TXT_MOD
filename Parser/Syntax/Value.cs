using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser.Syntax
{
    public class Value
    {

        protected static List<Value> LoadList(string raw, ref int charIndex)
        {
            List<Value> rslt = new List<Value>();

            int start = charIndex;
            int end = start;

            while (end < raw.Length)
            {
                var elemType = RegexMatch(raw, start, out end);

                switch (elemType)
                {
                    case ELEM_TYPE.KEY:
                        {
                            var node = new SyntaxItem(raw, ref start);
                            rslt.Add(node);
                        }
                        break;
                    case ELEM_TYPE.CR:
                        {
                            start = end;
                            continue;
                        }
                    case ELEM_TYPE.STRING:
                        {
                            var substr = raw.Substring(start, end - start);

                            decimal dbValue;
                            bool bValue;
                            if (decimal.TryParse(substr, out dbValue))
                            {
                                rslt.Add(new DigitValue() { digit = dbValue });
                            }
                            else if (bool.TryParse(substr, out bValue))
                            {
                                rslt.Add(new BoolValue() { data = bValue });
                            }
                            else
                            {
                                rslt.Add(new StringValue() { data = substr });
                            }

                            start = end;

                        }
                        break;
                    case ELEM_TYPE.COMMA:
                        {
                            if (rslt.Count == 0)
                            {
                                throw new Exception();
                            }
                            start = end;
                        }
                        break;
                    case ELEM_TYPE.BRACE_CLOSE:
                        {
                            charIndex = end;

                            return rslt;
                        }
                    default:
                        throw new Exception();

                }
            }

            return rslt;
        }

        internal static ELEM_TYPE RegexMatch(string raw, int start, out int end)
        {
            var curr = raw.Substring(start);

            var rslt = Regex.Match(curr, @"^\r?\n");
            if (rslt.Success)
            {
                end = start + rslt.Length;
                return ELEM_TYPE.CR;
            }

            rslt = Regex.Match(curr, @"^,");
            if (rslt.Success)
            {
                end = start + rslt.Length;
                return ELEM_TYPE.COMMA;
            }

            rslt = Regex.Match(curr, @"^{");
            if (rslt.Success)
            {
                end = start + rslt.Length;
                return ELEM_TYPE.BRACE_OPEN;
            }

            //rslt = Regex.Match(curr, @"^,");
            //if (rslt.Success)
            //{
            //    endIndex = charIndex + rslt.Length;
            //    return ELEM_TYPE.COMMA;
            //}

            rslt = Regex.Match(curr, @"^}");
            if (rslt.Success)
            {
                end = start + rslt.Length;
                return ELEM_TYPE.BRACE_CLOSE;
            }

            rslt = Regex.Match(curr, @"^[A-Za-z0-9_\.\+\-\*/%]+=");
            if (rslt.Success)
            {
                end = start + rslt.Length;
                return ELEM_TYPE.KEY;
            }

            rslt = Regex.Match(curr, @"^[A-Za-z0-9_\.\+\-\*/%]+");
            if (rslt.Success)
            {
                end = start + rslt.Length;
                return ELEM_TYPE.STRING;
            }

            throw new Exception();
        }
    }
}
