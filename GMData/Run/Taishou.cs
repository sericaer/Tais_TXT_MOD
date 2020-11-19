using DataVisit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Taishou : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DataVisitorProperty("is_revoke")]
        public bool isRevoke;

        [DataVisitorProperty("name"), JsonProperty]
        public string name;

        [DataVisitorProperty("age"), JsonProperty]
        public int age { get; set; }

        [JsonProperty]
        public string partyName;

        public Taishou(Init.Taishou init) : this()
        {
            this.name = init.name;
            this.age = init.age;
            this.partyName = init.party;
        }

        [JsonConstructor]
        private Taishou()
        {
            isRevoke = false;
        }
    }
}
