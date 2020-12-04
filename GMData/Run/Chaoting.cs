using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;
using ReactiveMarbles.PropertyChanged;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Chaoting : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        internal static Func<Def.IChaoting> getDef;

        [JsonProperty]
        public int reportPopNum { get; set; }

        [JsonProperty]
        public int requestTaxLevel { get; set; }

        public decimal monthTaxRequest { get; private set; }

        [JsonProperty]
        internal string powerPartyName;

        [DataVisitorProperty("extra_tax")]
        public decimal extraTax => _extraTax > 0 ? _extraTax : 0;

        [DataVisitorProperty("owe_tax")]
        public decimal oweTax => _extraTax < 0 ? _extraTax * -1 : 0;

        [DataVisitorProperty("year_report_tax")]
        public decimal yearReportTax { get; set; }

        [DataVisitorProperty("year_request_tax")]
        public decimal yearRequestTax { get; set; }

        public ObsBufferedValue monthTaxReqort;
        

        [JsonProperty]
        private decimal _extraTax => yearReportTax - yearRequestTax;

        internal void DaysInc(Date date)
        {
            if(date.day == 30)
            {
                yearReportTax += monthTaxReqort.value;
                yearRequestTax += monthTaxRequest;
            }
        }

        internal Chaoting(decimal popNum) : this()
        {
            var def = getDef();

            powerPartyName = def.powerParty;
            reportPopNum = (int)(popNum * (decimal)def.reportPopPercent / 100);
            requestTaxLevel = def.taxInfo.init_level;

            DataReactive(new StreamingContext());
        }

        [OnDeserialized]
        internal void DataReactive(StreamingContext context)
        {        
            Observable.CombineLatest(this.OBSProperty(x=>x.requestTaxLevel), this.OBSProperty(x => x.reportPopNum), CalcTax)
                      .Subscribe(sum=> monthTaxRequest = sum);

            this.OBSProperty(x => x.monthTaxRequest).Subscribe(x => monthTaxReqort.baseValue = x);
        }


        [JsonConstructor]
        private Chaoting()
        {
            monthTaxReqort = new ObsBufferedValue();
        }

        internal decimal CalcTax(int level, int num)
        {
            if(level == 0)
            {
                return 0;
            }

            var def = getDef();

            return (decimal)def.taxInfo.levels[level - 1].per_person * num;
        }
    }
}
