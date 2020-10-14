using DataVisit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Taishou
    {
        [DataVisitorProperty("is_revoke")]
        public bool isRevoke;

        [DataVisitorProperty("name"), JsonProperty]
        public string name;

        [DataVisitorProperty("age"), JsonProperty]
        public SubjectValue<int> age;

        [DataVisitorProperty("party")]
        public Party party
        { 
            get
            {
                return GMRoot.runner.partys.Find(x => x.name == partyName);
            }
        }


        [JsonProperty]
        internal string partyName;

        public Taishou(Init.Taishou init) : this()
        {
            this.name = init.name;
            this.age = new SubjectValue<int>(init.age);
            this.partyName = init.party;
        }

        [JsonConstructor]
        private Taishou()
        {
            isRevoke = false;
        }

    }
}
