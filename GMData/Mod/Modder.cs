using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GMData.Def;

namespace GMData.Mod
{
    public class Modder
    {
        public IEnumerable<Language> languages
        {
            get
            {
                return mods[0].languages;
            }
        }

        public IEnumerable<GEvent> events
        {
            get
            {
                foreach (var eventObj in mods[0].events.Values)
                {
                    foreach(var rslt in eventObj.Check())
                    {
                        yield return rslt;
                    }
                }
            }
        }

        public IEnumerable<InitSelect> initSelects
        {
            get
            {
                return mods[0].initSelects;
            }
        }

        public GEvent GetEvent(string key)
        {
            return mods[0].events[key];
        }

        public IEnumerable<Warn> warns
        {
            get
            {
                foreach (var warnObj in mods[0].warns)
                {
                    if(warnObj.isValid())
                    {
                        yield return warnObj;

                    }
                }
            }
        }

        public PersonName personName
        {
            get
            {
                return mods[0].personName;
            }
        }

        public List<Party> parties
        {
            get
            {
                return mods[0].parties;
            }
        }

        public List<Depart> departs
        {
            get
            {
                return mods[0].departs;
            }
        }

        public List<Pop> pops
        {
            get
            {
                return mods[0].pops;
            }
        }

        public Common common
        {
            get
            {
                return mods[0].common;
            }
        }

        public List<Risk> risks
        {
            get
            {
                return mods[0].risks;
            }
        }

        public List<Adjust> adjusts
        {
            get
            {
                return mods[0].adjusts;
            }
        }

        private List<Mod> mods;

        public Modder(string path)
        {
            mods = new List<Mod>();

            foreach (var sub in Directory.EnumerateDirectories(path))
            {
                var modname = Path.GetFileName(sub);
                mods.Add(new Mod(modname, sub));
            }

            GMRoot.define = new Define()
            {
                parties = this.parties,
                personName = this.personName,
                departs = this.departs,
                pops = this.pops,
                economy = this.common.economy,
                chaoting = this.common.chaoting,
                risks = this.risks,
                adjusts = this.adjusts
            };
        }

        public GEvent FindEvent(string key)
        {
            return mods[0].events.Single(x => x.Key == key).Value;
        }

    }

    internal class Mod
    {
        internal string name;
        internal string path;

        internal List<Language> languages;
        internal Dictionary<string, GEvent> events;
        internal List<InitSelect> initSelects;
        internal List<Party> parties;
        internal List<Depart> departs;
        internal List<Pop> pops;
        internal List<Warn> warns;
        internal List<Risk> risks;
        internal List<Adjust> adjusts;

        internal PersonName personName;
        internal Common common;

        public Mod(string name, string path)
        {
            this.name = name;
            this.path = path;

            languages = Language.Load(path + "/language");
            initSelects = InitSelect.Load(name, path + "/init_select");
            personName = PersonName.Load(path + "/person_name");
            parties = Party.Load(name, path + "/party");
            departs = Depart.Load(name, path + "/depart");
            pops = Pop.Load(name, path + "/pop");
            common = Common.Load(name, path + "/common");
            events = GEvent.Load(name, path + "/event");
            warns = Warn.Load(name, path + "/warn");
            risks = Risk.Load(name, path + "/risk");
            adjusts = Adjust.Load(name, path + "/adjust");
        }
    }
}
