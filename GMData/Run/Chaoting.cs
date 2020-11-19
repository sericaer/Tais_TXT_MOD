using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Chaoting
    {
        [JsonProperty]
        public SubjectValue<int> reportPopNum;

        [JsonProperty]
        public SubjectValue<int> requestTaxLevel;

        public OBSValue<int> reportTaxLevel;

        public OBSValue<decimal> monthTaxRequest;

        public OBSValue<decimal> monthTaxReqort;

        [JsonProperty]
        internal string powerPartyName;

        [DataVisitorProperty("extra_tax")]
        public decimal extraTax => _extraTax > 0 ? _extraTax : 0;

        [DataVisitorProperty("owe_tax")]
        public decimal oweTax => _extraTax < 0 ? _extraTax * -1 : 0;

        [JsonProperty]
        private decimal _extraTax;


        internal static void DaysInc()
        {

        }

        internal Chaoting(Def.Chaoting def, decimal popNum) : this()
        {
            powerPartyName = def.powerParty;
            reportPopNum = new SubjectValue<int>((int)(popNum * (decimal)def.reportPopPercent / 100));
            requestTaxLevel = new SubjectValue<int>(def.tax_level);

            DataReactive(new StreamingContext());
        }

        internal void ReportMonthTax(decimal value)
        {
            _extraTax += value - monthTaxRequest.Value;
        }

        internal void ReportTaxPlus(decimal value)
        {
            _extraTax += value;
        }

        [OnDeserialized]
        internal void DataReactive(StreamingContext context)
        {        
            Observable.CombineLatest(requestTaxLevel.obs, reportPopNum.obs, CalcTax)
                      .Subscribe(monthTaxRequest);
            Observable.CombineLatest(reportTaxLevel.obs, reportPopNum.obs, CalcTax)
                      .Subscribe(monthTaxReqort);
        }


        [JsonConstructor]
        private Chaoting()
        {
            monthTaxRequest = new OBSValue<decimal>();
            monthTaxReqort = new OBSValue<decimal>();
            reportTaxLevel = new OBSValue<int>();
        }

        internal decimal CalcTax(int level, int num)
        {
            var levels = GMRoot.define.adjusts.Single(x => x.key == "REPORT_CHAOTING").levels;
            return (decimal)levels[level - 1].percent * 0.01M * num * 0.006M;
        }

    }
}
