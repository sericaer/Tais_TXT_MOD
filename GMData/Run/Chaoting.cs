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
        public SubjectValue<double> reportTaxPercent;

        [JsonProperty]
        internal string powerPartyName;

        public ObservableValue<double> expectMonthTaxValue;

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
            reportTaxPercent = new SubjectValue<double>(def.taxPercent);

            DataReactive(new StreamingContext());
        }

        internal void ReportMonthTax(double value)
        {
            _extraTax += value - expectMonthTaxValue.Value;
        }

        internal void ReportTaxPlus(double value)
        {
            _extraTax += value;
        }

        [OnDeserialized]
        internal void DataReactive(StreamingContext context)
        {
            expectMonthTaxValue = Observable.CombineLatest(reportPopNum.obs, reportTaxPercent.obs,
                                        (x, y) => x * 0.006 * y / 100)
                                        .ToOBSValue();
        }

        [JsonConstructor]
        private Chaoting()
        {
        }
    }
}
