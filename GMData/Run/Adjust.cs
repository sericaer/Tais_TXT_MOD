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
    public class Adjust
    {
        public enum EType
        {
            POP_TAX,
            REPORT_CHAOTING,
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

    }
}
