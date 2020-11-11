using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DataVisit;
using Newtonsoft.Json;

namespace GMData.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Adjust
    {
        public enum EType
        {
            [EconomyInput]
            POP_TAX,

            [EconomyOutput]
            REPORT_CHAOTING,

            [EconomyOutput]
            ADMIN_SPEND
        }

        [JsonProperty]
        public EType etype;

        [JsonProperty]
        public SubjectValue<int> level;

        [JsonProperty]
        public SubjectValue<bool> valid;

        public Adjust(EType eType)
        {
            this.etype = eType;
            this.level = new SubjectValue<int>(def.init.level);
            this.valid = new SubjectValue<bool>(def.init.valid);
        }

        [JsonConstructor]
        private Adjust()
        {

        }
        
        public GMData.Def.Adjust.Level levelDef
        {
            get
            {
                return def.levels[level.Value-1];
            }
        }

        public GMData.Def.Adjust def
        {
            get
            {
                return GMRoot.define.adjusts.Single(x => x.key == etype.ToString());
            }
        }

        public class EconomyInput : Attribute
        {
        }

        public class EconomyOutput : Attribute
        {
        }
    }
}
