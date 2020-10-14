using System;
using System.IO;
namespace UnitTest.Modder.Mock
{
    public class ModFileSystem
    {
        public static string path = "../../test_data/";

        public string name;
        public string modPath
        {
            get
            {
                return path + name + "/";
            }
        }

        public string commonPath
        {
            get
            {
                return modPath + "events/common/"; ;
            }
        }

        internal void AddCommonEvent(string fileName, string fileContent)
        {
            Directory.CreateDirectory(commonPath);

            File.WriteAllText(commonPath + fileName, fileContent);
        }

        internal void AddDepartEvent(string fileName, string fileContent)
        {
            var commonPath = modPath + "events/depart/";
            Directory.CreateDirectory(commonPath);

            File.WriteAllText(commonPath + fileName, fileContent);
        }

        internal void AddCommonWarn(string fileName, string fileContent)
        {
            var commonPath = modPath + "warns/common/";
            Directory.CreateDirectory(commonPath);

            File.WriteAllText(commonPath + fileName, fileContent);
        }

        internal void AddRisk(string fileName, string fileContent)
        {
            var initSelectPath = modPath + "risks/";
            Directory.CreateDirectory(initSelectPath);

            File.WriteAllText(initSelectPath + fileName, fileContent);
        }

        internal void AddDepartWarn(string fileName, string fileContent)
        {
            var commonPath = modPath + "warns/depart/";
            Directory.CreateDirectory(commonPath);

            File.WriteAllText(commonPath + fileName, fileContent);
        }

        internal void AddInitSelect(string fileName, string fileContent)
        {
            var initSelectPath = modPath + "init_select/";
            Directory.CreateDirectory(initSelectPath);

            File.WriteAllText(initSelectPath + fileName, fileContent);
        }

        public static ModFileSystem Generate(string modName)
        {
            return new ModFileSystem() { name = modName };
        }

        internal static void Clear()
        {
            if(!Directory.Exists(path))
            {
                return;
            }
            foreach(var dir in Directory.EnumerateDirectories(path))
            {
                Directory.Delete(dir, true);
            }
            
        }
    }
}
