using DataVisit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.Serialization;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Date : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Inc()
        {
            if (day != 30)
            {
                day++;
            }
            else if (month != 12)
            {
                day = 1;
                month++;
                return;
            }
            else
            {
                month = 1;
                day = 1;
                year++;
            }
        }

        public static bool operator ==(Date l, (int? year, int? month, int? day) r)
        {
            if (r.year != null && l.year != r.year.Value)
            {
                return false;
            }
            if (r.month != null && l.month != r.month.Value)
            {
                return false;
            }
            if (r.day != null && l.day != r.day.Value)
            {
                return false;
            }

            return true;
        }

        public static bool operator >(Date l, (int? year, int? month, int? day) r)
        {
            if (r.year != null)
            {
                if (l.year > r.year.Value)
                    return true;
                if (l.year < r.year.Value)
                    return false;
            }
            if (r.month != null)
            {
                if (l.month > r.month.Value)
                    return true;
                if (l.month < r.month.Value)
                    return false;
            }
            if (r.day != null)
            {
                if (l.day > r.day.Value)
                    return true;
                if (l.day < r.day.Value)
                    return false;
            }

            return false;
        }

        public static bool operator <(Date l, (int? year, int? month, int? day) r)
        {
            return !(l > r || l == r);
        }

        public static bool operator <=(Date l, (int? year, int? month, int? day) r)
        {
            return l < r || l == r;
        }

        public static bool operator >=(Date l, (int? year, int? month, int? day) r)
        {
            return l > r || l == r;
        }

        public static bool operator !=(Date l, (int? year, int? month, int? day) r)
        {
            return !(l == r);
        }

        public static bool operator ==(Date l, Date r)
        {
            return l == (r.year, r.month, r.day);
        }

        public static bool operator !=(Date l, Date r)
        {
            return !(l == r);
        }

        public static bool operator >(Date l, Date r)
        {
            return l > (r.year, r.month, r.day);
        }

        public static bool operator <(Date l, Date r)
        {
            return !(l > r || l == r);
        }

        public static bool operator >=(Date l, Date r)
        {
            return l > r || l == r;
        }

        public static bool operator <=(Date l, Date r)
        {
            return l < r || l == r;
        }

        [JsonProperty, DataVisitorProperty("year")]
        public int year { get; set; }

        [JsonProperty, DataVisitorProperty("month")]
        public int month { get; set; }

        [JsonProperty, DataVisitorProperty("day")]
        public int day { get; set; }

        public string desc { get; private set; }

        public int total_days { get; private set; }

        //public int total_days
        //{
        //    get
        //    {
        //        return day.Value + (month.Value - 1) * 12 + (year.Value - 1) * 360;
        //    }
        //}

        public Date(Init.Date init) : this()
        {
            year = (int)init.year;
            month = (int)init.month;
            day = (int)init.day;

            DataAssociate(new StreamingContext());
        }

        [OnDeserialized]
        internal void DataAssociate(StreamingContext context)
        {
            Observable.Merge(this.OBSProperty(x => x.year), 
                           this.OBSProperty(x => x.month), 
                           this.OBSProperty(x => x.day))
                .Subscribe(_ =>
                {
                    desc = $"{year}-{month}-{day}";
                    total_days = day + (month - 1) * 30 + (year - 1) * 360;
                });
        }

        [JsonConstructor]
        private Date()
        {
        }
    }
}