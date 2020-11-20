using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class Adjust : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

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
        public int level { get; set; }

        [JsonProperty]
        public bool valid { get; set; }

        public Adjust(EType eType)
        {
            this.etype = eType;
            this.level = def.init.level;
            this.valid = def.init.valid;
        }

        [JsonConstructor]
        private Adjust()
        {

        }
        
        public GMData.Def.Adjust.Level levelDef
        {
            get
            {
                return def.levels[level-1];
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
