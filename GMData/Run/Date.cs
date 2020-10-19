using DataVisit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Date
    {
        public void Inc()
        {
            if (day.Value != 30)
            {
                day.Value++;
            }
            else if (month.Value != 12)
            {
                day.Value = 1;
                month.Value++;
                return;
            }
            else
            {
                month.Value = 1;
                day.Value = 1;
                year.Value++;
            }
        }

        public static bool operator ==(Date l, (int? year, int? month, int? day) r)
        {
            if (r.year != null && l.year.Value != r.year.Value)
            {
                return false;
            }
            if (r.month != null && l.month.Value != r.month.Value)
            {
                return false;
            }
            if (r.day != null && l.day.Value != r.day.Value)
            {
                return false;
            }

            return true;
        }

        public static bool operator >(Date l, (int? year, int? month, int? day) r)
        {
            if (r.year != null)
            {
                if (l.year.Value > r.year.Value)
                    return true;
                if (l.year.Value < r.year.Value)
                    return false;
            }
            if (r.month != null)
            {
                if (l.month.Value > r.month.Value)
                    return true;
                if (l.month.Value < r.month.Value)
                    return false;
            }
            if (r.day != null)
            {
                if (l.day.Value > r.day.Value)
                    return true;
                if (l.day.Value < r.day.Value)
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
            return l == (r.year.Value, r.month.Value, r.day.Value);
        }

        public static bool operator !=(Date l, Date r)
        {
            return !(l == r);
        }

        public static bool operator >(Date l, Date r)
        {
            return l > (r.year.Value, r.month.Value, r.day.Value);
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
        public SubjectValue<int> year;

        [JsonProperty, DataVisitorProperty("month")]
        public SubjectValue<int> month;

        [JsonProperty, DataVisitorProperty("day")]
        public SubjectValue<int> day;

        public ObservableValue<string> desc;

        public ObservableValue<int> total_days;

        //public int total_days
        //{
        //    get
        //    {
        //        return day.Value + (month.Value - 1) * 12 + (year.Value - 1) * 360;
        //    }
        //}

        public Date(Init.Date init)
        {
            year = new SubjectValue<int>((int)init.year);
            month = new SubjectValue<int>((int)init.month);
            day = new SubjectValue<int>((int)init.day);
        }

        internal void DataAssociate()
        {
            desc = Observable.CombineLatest(year.obs, month.obs, day.obs, (y, m, d) => $"{y}-{m}-{d}").ToOBSValue();
            total_days = Observable.CombineLatest(year.obs, month.obs, day.obs, (y, m, d) => d + (m - 1) * 30 + (y - 1) * 360).ToOBSValue();
        }

        [JsonConstructor]
        private Date()
        {
        }
    }
}