using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DynamicData;
using Newtonsoft.Json;
using System;
using System.Reactive.Linq;


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
        internal ObsBufferedValue relation { get; set; }

        [JsonProperty]
        public Person[] persons
        {
            get
            {
                return _persons.Items.ToArray();
            }
            set
            {
                foreach(var elem in value)
                {
                    elem.family = this;
                }

                _persons.Edit(list =>
                {
                    list.Clear();
                    list.AddRange(value);
                });
            }
        }

        public Pop pop;

        private SourceList<Person> _persons;

        public Family(int person_count, string partyName) : this()
        {
            this.partyName = partyName;
            this.persons = GMRoot.define.personName.GetRandomPersonArray(person_count).Select(x => new Person(x)).ToArray();

            DataReactive(new StreamingContext());
        }

        [JsonConstructor]
        private Family()
        {
            this.relation = new ObsBufferedValue((_, buffs) => buffs.Sum() / buffs.Count());
            this._persons = new SourceList<Person>();
        }

        [OnDeserialized]
        private void DataReactive(StreamingContext context)
        {
            _persons.Connect().WhenPropertyChanged(p => p.relation.value).Subscribe(x =>
            {
                this.relation.SetBuffer(x.Sender.fullName, x.Value);
            });
        }

        internal Person GeneratePerson()
        {
            var person = new Person(GMRoot.define.personName.given.First(x=>persons.All(p=>p.givenName != x)));
            person.family = this;

            _persons.Add(person);
            return person;
        }
    }
}