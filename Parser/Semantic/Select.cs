using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class Select
    {
        public static Select Parse(SyntaxItem item)
        {
            var rslt = new Select();

            foreach(var elem in item.values)
            {
                var subItem = elem as SyntaxItem;
                if(subItem == null)
                {
                    throw new Exception("must key-value pair in select");
                }
                switch(subItem.key)
                {
                    case "reduce":
                        rslt.opList.Add(OperationReduce.Parse(subItem));
                        break;
                    case "add":
                        rslt.opList.Add(OperationAdd.Parse(subItem));
                        break;
                    case "assign":
                        rslt.opList.Add(OperationAssign.Parse(subItem));
                        break;
                    case "risk.start":
                        rslt.opList.Add(OperationRiskStart.Parse(subItem));
                        break;
                    default:
                        throw new Exception($"can not support {subItem.key} in select");
                }
            }

            return rslt;
        }


        public void Do()
        {
            opList.ForEach(x => x.Do());
        }

        public (string op, string left, SingleValue right)[] info
        {
            get
            {
                return opList.Select(x => (x.GetType().Name, x.left.ToString(), x.right)).ToArray();
            }
        }

        internal List<Operation> opList = new List<Operation>();
    }
}
