using DataVisit;

namespace GMData.Init
{
    public class Taishou
    {
        [DataVisitorProperty("name")]
        public string name;

        [DataVisitorProperty("age")]
        public int age;

        [DataVisitorProperty("party")]
        public string party;
    }
}