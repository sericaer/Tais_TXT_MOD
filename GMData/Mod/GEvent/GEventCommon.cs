using System;
using System.Collections.Generic;
using System.IO;

namespace GMData.Mod
{
    public class GEventCommon : GEvent
    {
        public GEventCommon(string file) : base(file)
        {
        }

        internal static void Load(string name, string path, ref Dictionary<string, GEvent> dict)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var eventobj = new GEventCommon(file);
                dict.Add(eventobj.key, eventobj);
            }
        }

        internal override IEnumerable<GEvent> Check()
        {
            if(isValid())
            {
                yield return this;
            }
        }
    }
}
