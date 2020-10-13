using Parser.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Semantic
{
    public class GroupValue : IEnumerable<object>
    {
        List<SingleValue> datas = new List<SingleValue>();

        public static GroupValue Parse(SyntaxItem item)
        {
            var Params = new List<SingleValue>();
            foreach(var value in item.values)
            {
                var stringValue = value as SingleValue;
                if(stringValue == null)
                {
                    throw new Exception($"Semantic error, {item} value must be single value");
                }

                Params.Add(stringValue);
            }

            return new GroupValue(Params.ToArray());
        }
        public GroupValue(params SingleValue[] Params)
        {
            datas.AddRange(Params);
        }

        public IEnumerator<object> GetEnumerator()
        {
            for(int i=0; i< datas.Count(); i++)
            {
                yield return Visitor.GetValue(datas[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < datas.Count(); i++)
            {
                yield return Visitor.GetValue(datas[i]);
            }
        }
    }
}