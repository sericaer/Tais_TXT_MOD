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
            Visitor.InitVisitMap(typeof(Init.InitData));
            Visitor.InitVisitMap(typeof(Run.Runner));

            Parser.Semantic.Visitor.SetValueFunc = Visitor.Set;
            Parser.Semantic.Visitor.GetValueFunc = Visitor.Get;
        }

        public static Action<object[]> logger;
        public static Mod.Modder modder;
        public static Run.Runner runner;
        public static Init.InitData initData;

        public static Def.Define define;
    }
}
