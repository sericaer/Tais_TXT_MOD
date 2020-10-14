using System;
using DataVisit;

namespace GMData.Init
{
    public class InitData
    {
        [DataVisitorProperty("taishou")]
        public Taishou taishou;

        public InitData()
        {
            taishou = new Taishou();

            Visitor.SetVisitData(this);
        }
    }
}
