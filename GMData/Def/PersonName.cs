using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GMData.Def
{
    public class PersonName
    {

        //internal static IEnumerable<string> EnumFamily()
        //{
        //    foreach (var mod in Mod.listMod.Where(x => x.content != null))
        //    {
        //        if (!mod.content.localString.dictlan2PersonName.ContainsKey(Config.inst.lang))
        //        {
        //            continue;
        //        }

        //        foreach (var name in mod.content.localString.dictlan2PersonName[Config.inst.lang].family)
        //        {
        //            yield return name;
        //        }
        //    }
        //}

        //internal static IEnumerable<string> EnumGiven()
        //{
        //    foreach (var mod in Mod.listMod.Where(x => x.content != null))
        //    {
        //        if (!mod.content.localString.dictlan2PersonName.ContainsKey(Config.inst.lang))
        //        {
        //            continue;
        //        }

        //        foreach (var name in mod.content.localString.dictlan2PersonName[Config.inst.lang].given)
        //        {
        //            yield return name;
        //        }
        //    }
        //}

        public string RandomFull
        {
            get
            {
                return RandomFamily + RandomGiven;
            }
        }

        public string RandomFamily
        {
            get
            {
                return family.OrderBy(x => Guid.NewGuid()).First();
            }
        }

        public string RandomGiven
        {
            get
            {
                return given.OrderBy(x => Guid.NewGuid()).First();
            }
        }

        public static PersonName Load(string dir)
        {
            var familyNamePath = $"{dir}/family.txt";
            var givenNamePath = $"{dir}/given.txt";

            string[] family = { };
            if (File.Exists(familyNamePath))
            {
                family = File.ReadAllLines(familyNamePath);
            }

            string[] given = { };
            if (File.Exists(givenNamePath))
            {
                given = File.ReadAllLines(givenNamePath);
            }

            if (family.Count() == 0 && given.Count() == 0)
            {
                return null;
            }

            return new PersonName(family, given);
        }

        internal string[] GetRandomFamilyArray(int count)
        {
            return family.OrderBy(x => Guid.NewGuid()).Take(count).ToArray();
        }

        internal string[] GetRandomPersonArray(int count)
        {
            return given.OrderBy(x => Guid.NewGuid()).Take(count).ToArray();
        }

        internal PersonName(string[] family, string[] given)
        {
            this.family = family;
            this.given = given;
        }

        internal string[] family;
        internal string[] given;
    }
}
