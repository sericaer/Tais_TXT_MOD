using System;
using DataVisit;

namespace GMData.Init
{
    public class InitData
    {
        [DataVisitorProperty("taishou")]
        public Taishou taishou;

        [DataVisitorProperty("start_date")]
        public Date start_date;

        public Economy economy;

        public InitData()
        {
            taishou = new Taishou();
            start_date = new Date() { year = 1, month = 1, day = 1 };
            economy = new Economy() { curr = 200,
                                      expend_depart_admin = 80,
                                      pop_tax_percent = 20,
                                      report_chaoting_percent = 100
                                    };
            Visitor.SetVisitData(this);
        }
    }
}
