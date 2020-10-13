using System;
using System.Collections.Generic;
using System.IO;

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
        }

        public IEnumerable<GEvent> events { get; }
    }

    internal class Mod
    {
        internal string name;
        internal string path;

        internal List<Language> languages;

        public Mod(string name, string path)
        {
            this.name = name;
            this.path = path;

            languages = Language.Load(path + "/language");
        }
    }
}
