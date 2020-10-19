using System;
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
        public static Func<string, Party> funcGetParty;

        [JsonProperty]
        public SubjectValue<int> reportPopNum;

        [JsonProperty]
        public SubjectValue<double> reportTaxPercent;

        public ObservableValue<double> expectMonthTaxValue;

        [DataVisitorProperty("extra_tax")]
        public double extraTax
        {
            get
            {
                return _extraTax > 0 ? _extraTax : 0;
            }
        }

        [DataVisitorProperty("owe_tax")]
        public double oweTax
        {
            get
            {
                return _extraTax < 0 ? _extraTax*-1 : 0;
            }
        }

        [JsonProperty]
        private double _extraTax;

        [DataVisitorProperty("power_party")]
        public Party powerParty => funcGetParty(powerPartyName);


        [JsonProperty]
        internal string powerPartyName;

        internal static void DaysInc()
        {

        }

        internal Chaoting(Def.Chaoting def, double popNum)
        {
            powerPartyName = def.powerParty;
            if(powerParty == null)
            {
                throw new Exception($"can not find chaoting power party ${powerPartyName}");
            }

            reportPopNum = new SubjectValue<int>((int)(popNum * def.reportPopPercent / 100));
            reportTaxPercent = new SubjectValue<double>(def.taxPercent);
        }

        internal void ReportMonthTax(double value)
        {
            _extraTax += value - expectMonthTaxValue.Value;
        }

        internal void DataAssocate()
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
