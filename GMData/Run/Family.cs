using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Family
    {
        [JsonProperty]
        internal string name;

        [JsonProperty]
        internal string partyName;

        [JsonProperty]
        public ObsBufferedValue relation;

        [JsonProperty]
        public List<Person> persons
        {
            get
            {
                return _persons;
            }
            set
            {
                _persons = value;
                _persons.ForEach(x => x.family = this);
            }
        }

        public Pop pop;

        private List<Person> _persons;

        public Family(int person_count, string partyName)
        {
            this.partyName = partyName;

            var names = GMRoot.define.personName.GetRandomPersonArray(person_count);
            persons = names.Select(x => new Person(x)).ToList();

            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Family()
        {

        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
        }
    }
}