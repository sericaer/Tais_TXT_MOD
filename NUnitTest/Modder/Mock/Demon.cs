using System;
using System.Collections.Generic;
using DataVisit;

namespace UnitTest.Modder.Mock
{
    public class Demon
    {
        public static Demon inst;
        public static object Init()
        {
            inst = new Demon();
            return inst;
        }

        [DataVisitorProperty("item1")]
        public Item1 item1;

        [DataVisitorPropertyArray("depart")]
        public List<Depart> departs;

        public Demon()
        {
            item1 = new Item1();

            departs = new List<Depart>() { new Depart(),
                                           new Depart()};
        }
    }

    public class Item1
    {
        [DataVisitorProperty("data1")]
        public double data1;

        [DataVisitorProperty("data2")]
        public double data2;


        [DataVisitorProperty("data3")]
        public bool data3;
    }

    public class Depart
    {
        [DataVisitorProperty("name")]
        public string name;

        [DataVisitorProperty("data1")]
        public double data1;

        [DataVisitorProperty("data2")]
        public double data2;
    }
}
