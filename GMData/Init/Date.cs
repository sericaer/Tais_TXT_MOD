using System;
using DataVisit;

namespace GMData.Init
{
    public class Date
    {
        [DataVisitorProperty("day")]
        public double day;

        [DataVisitorProperty("month")]
        public double month;

        [DataVisitorProperty("year")]
        public double year;
    }
}
