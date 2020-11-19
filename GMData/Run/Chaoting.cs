using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Chaoting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty]
        public int reportPopNum { get; set; }

        [JsonProperty]
        public int requestTaxLevel { get; set; }

        public int reportTaxLevel { get; set; }

        public decimal monthTaxRequest { get; private set; }

        public decimal monthTaxReqort { get; private set; }

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
            reportPopNum = (int)(popNum * (decimal)def.reportPopPercent / 100);
            requestTaxLevel = def.tax_level;

            DataReactive(new StreamingContext());
        }

        internal void ReportMonthTax(decimal value)
        {
            _extraTax += value - monthTaxRequest;
        }

        internal void ReportTaxPlus(decimal value)
        {
            _extraTax += value;
        }

        [OnDeserialized]
        internal void DataReactive(StreamingContext context)
        {        
            Observable.CombineLatest(this.OBSProperty(x=>x.requestTaxLevel), this.OBSProperty(x => x.reportPopNum), CalcTax)
                      .Subscribe(sum=> monthTaxRequest = sum);
            Observable.CombineLatest(this.OBSProperty(x => x.reportTaxLevel), this.OBSProperty(x => x.reportPopNum), CalcTax)
                      .Subscribe(sum => monthTaxReqort = sum);
        }


        [JsonConstructor]
        private Chaoting()
        {
        }

        internal decimal CalcTax(int level, int num)
        {
            if(level == 0)
            {
                return 0;
            }

            var levels = GMRoot.define.adjusts.Single(x => x.key == "REPORT_CHAOTING").levels;
            return (decimal)levels[level - 1].percent * 0.01M * num * 0.006M;
        }

    }
}
