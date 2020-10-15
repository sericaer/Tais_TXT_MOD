using System;
using System.Collections.Generic;

namespace GMData.Def
{
    public class Depart
    {
        public Depart()
        {
        }

        public string key;
        public COLOR color;
        public List<POP_INIT> pop_init;

        public class POP_INIT
        {
            public string name;
            public double num;
        }

        public class COLOR
        {
            public double r;
            public double g;
            public double b;
        }
    }
}
