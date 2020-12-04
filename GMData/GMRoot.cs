using System;
using DataVisit;

namespace GMData
{
    public class GMRoot
    {
        static GMRoot()
        {
            Init();
        }

        public static void Init()
        {
            AssocTypeStatic();

            Visitor.InitVisitMap(typeof(Init.InitData));
            Visitor.InitVisitMap(typeof(Run.Runner));

            Parser.Semantic.Visitor.SetValueFunc = Visitor.Set;
            Parser.Semantic.Visitor.GetValueFunc = Visitor.Get;
            Parser.Semantic.Visitor.SetCurrObj = Visitor.SetCurr;
            Parser.Semantic.Visitor.RemoveCurrObj = Visitor.RemoveCurrObj;

            
        }

        public static void AssocTypeStatic()
        {
            Run.Risk.funcGetDef = (key) => define.risks.Find(x => x.key == key);
            Run.Chaoting.getDef = ()=> define.chaoting;
        }

        public static Action<object[]> logger;
        public static Mod.Modder modder;
        public static Run.Runner runner;
        public static Init.InitData initData;

        public static Def.Define define;
    }
}
