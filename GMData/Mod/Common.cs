using System;
using System.IO;
using GMData.Def;
using Parser.Semantic;

namespace GMData.Mod
{
    public class Common
    {
        [SemanticProperty("economy")]
        internal Economy economy;

        [SemanticProperty("chaoting")]
        internal Chaoting chaoting;

        public Common()
        {
        }

        internal static Common Load(string name, string path)
        {
            var file = path + "/common_define.txt";
            if(!File.Exists(file))
            {
                return new Common();
            }

            return ModElementLoader.Load<Common>(file, File.ReadAllText(file));
        }
    }
}
