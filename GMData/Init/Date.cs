using System;
using DataVisit;

namespace GMData.Init
{
    public class Date
    {
        [DataVisitorProperty("day")]
        public decimal day;

        [DataVisitorProperty("month")]
        public decimal month;

        [DataVisitorProperty("year")]
        public decimal year;
    }
}
