//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Parser.Syntax
//{
//    public class SyntaxRoot
//    {
//        public static int line;

//        public SyntaxRoot(String raw)
//        {
//            line = 0;

//            synatxNodes = new List<SyntaxItem>();

//            var trim = Regex.Replace(raw, @"[\f\r\t\v ]+", "");

//            int charIndex = 0;
//            while (charIndex < trim.Length)
//            {
//                var node = new SyntaxItem(trim, ref charIndex);
//                synatxNodes.Add(node);
//            }
//        }

//        public IEnumerable<SyntaxItem> Enumerate()
//        {
//            foreach (var node in synatxNodes)
//            {
//                yield return node;
//            }
//        }

//        public override string ToString()
//        {
//            return String.Join("\n", synatxNodes.Select(x => x.ToString()));
//        }

//        public SyntaxItem Find(string key)
//        {
//            return synatxNodes.SingleOrDefault(x => x.key == key);
//        }

//        public IEnumerable<SyntaxItem> Finds(string key)
//        {
//            return synatxNodes.Where(x => x.key == key);
//        }

//        List<SyntaxItem> synatxNodes;
//    }
//}
