using System;
using System.Collections.Generic;
using System.IO;
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
                    if (eventObj.isValid())
                    {
                        yield return eventObj;
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

        public PersonName personName
        {
            get
            {
                return mods[0].personName;
            }
        }

        public IEnumerable<Party> parties
        {
            get
            {
                return mods[0].parties;
            }
        }

        private List<Mod> mods;

        public IEnumerable<Warn> warns { get; }

        public Modder(string path)
        {
            mods = new List<Mod>();

            foreach (var sub in Directory.EnumerateDirectories(path))
            {
                var modname = Path.GetFileName(sub);
                mods.Add(new Mod(modname, sub));
            }

            GMRoot.define = new Define() {
                parties = this.mods[0].parties;
            }
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
        internal PersonName personName;

        public Mod(string name, string path)
        {
            this.name = name;
            this.path = path;

            languages = Language.Load(path + "/language");
            initSelects = InitSelect.Load(name, path + "/init_select");
            personName = PersonName.Load(path + "/person_name");
            parties = Party.Load(name, path + "party");
        }
    }
}
