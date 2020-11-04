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

        [JsonProperty]
        internal string powerPartyName;

        public ObservableValueEx<double> monthTaxRequest;

        public ObservableValueEx<double> monthTaxReqort;

        [DataVisitorProperty("extra_tax")]
        public double extraTax => _extraTax > 0 ? _extraTax : 0;

        [DataVisitorProperty("owe_tax")]
        public double oweTax => _extraTax < 0 ? _extraTax * -1 : 0;

        [JsonProperty]
        private double _extraTax;


        internal static void DaysInc()
        {

        }

        internal Chaoting(Def.Chaoting def, double popNum)
        {
            powerPartyName = def.powerParty;

            reportPopNum = new SubjectValue<int>((int)(popNum * def.reportPopPercent / 100));
            requestTaxLevel = new SubjectValue<int>(def.tax_level);

            monthTaxRequest = new ObservableValueEx<double>();
            monthTaxReqort = new ObservableValueEx<double>();

            DataReactive(new StreamingContext());
        }

        internal void ReportMonthTax(double value)
        {
            _extraTax += value - monthTaxRequest.Value;
        }

        internal void ReportTaxPlus(double value)
        {
            _extraTax += value;
        }

        [OnDeserialized]
        internal void DataReactive(StreamingContext context)
        {        
            Observable.CombineLatest(requestTaxLevel.obs, reportPopNum.obs, CalcTax)
                      .Subscribe(monthTaxRequest);
        }


        [JsonConstructor]
        private Chaoting()
        {
            monthTaxRequest = new ObservableValueEx<double>();
            monthTaxReqort = new ObservableValueEx<double>();
        }

        internal double CalcTax(int level, int num)
        {
            var levels = GMRoot.define.economy.outputs.Single(x => x.key == "CHAOTING").levels;
            return levels[level - 1].percent * 0.01 * num * 0.006;
        }

    }
}
