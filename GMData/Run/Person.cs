using System.ComponentModel;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Person : INotifyPropertyChanged
    {
        [JsonProperty]
        public readonly string givenName;

        [JsonProperty]
        public ObsBufferedValue relation { get; set; }

        internal Family family;

        public event PropertyChangedEventHandler PropertyChanged;

        public string familyName => family.name;
        public string fullName => family.name + givenName;

        public Person(string name) : this()
        {
            this.givenName = name;
            
        }

        [JsonConstructor]
        private Person()
        {
            this.relation = new ObsBufferedValue();
        }
    }
}