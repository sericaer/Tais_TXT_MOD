using System;
using System.Collections.Generic;
using Parser.Syntax;

namespace Parser.Semantic
{
    public abstract class Operation
    {
        //public static OpSet Parse(SyntaxItem item)
        //{
        //    var sub = new List<(string right, SingleValue left)>();

        //    foreach(var value in item.values)
        //    {
        //        var subItem = value as SyntaxItem;
        //        if(subItem  == null)
        //        {
        //            throw new Exception("set sub iteam must be key-value pair!");
        //        }
        //        if(subItem.values.Count != 1 || !(subItem.values[0] is SingleValue))
        //        {
        //            throw new Exception("set sub iteam must be have one StringValue!");
        //        }

        //        sub.Add((subItem.key, subItem.values[0] as SingleValue));
        //    }

        //    return new OpSet() { list = sub };
        //}

        //public void Do()
        //{
        //    list.ForEach(x => Visitor.SetValue(x.left, x.right));
        //}

        //public List<(string left, SingleValue right)> list;
        public abstract void Do();

        internal SingleValue left;
        internal SingleValue right;
    }
}
