using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Person
    {
        [JsonProperty]
        public readonly string givenName;

        [JsonProperty]
        public ObsBufferedValue relation;

        internal Family family;

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