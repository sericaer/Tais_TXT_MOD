using System;
using System.Collections.Generic;
using System.IO;

namespace GMData.Mod
{
    public class WarnCommon : Warn
    {
        internal static void Load(string name, string path, ref List<Warn> list)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var warn = ModElementLoader.Load<Warn>(file, File.ReadAllText(file));
                warn.file = file;

                list.Add(warn);
            }
        }
    }
}
