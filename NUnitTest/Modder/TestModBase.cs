using System;
using GMData.Mod;
using UnitTest.Modder.Mock;

namespace NUnitTest.Modder
{
    public class TestModBase
    {
        internal ModFileSystem modFileSystem;
        internal GMData.Mod.Modder modder;

        internal TestModBase()
        {
            ModFileSystem.Clear();

            modFileSystem = ModFileSystem.Generate(this.GetType().Name);
        }

        internal void LoadModScript(string path, params (string file, string content)[] events)
        {
            foreach (var fevent in events)
            {
                modFileSystem.AddFile(path, fevent.file, fevent.content);
            }

            modder = new GMData.Mod.Modder(ModFileSystem.path);
        }
    }
}
