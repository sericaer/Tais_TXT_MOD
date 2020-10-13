using System;

namespace GMData
{
    public class GMRoot
    {
        public static Action<object[]> logger;
        public static Mod.Modder modder;
        public static Run.Runner runner;

        public static Def.Define define = new Def.Define();
    }
}
