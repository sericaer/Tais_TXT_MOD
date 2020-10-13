//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace GMData.Mod
//{
//    internal class WarnGroup
//    {
//        internal List<Warn> _common = new List<Warn>();
//        internal List<Warn> _depart = new List<Warn>();
//        internal List<Warn> _pop = new List<Warn>();

//        internal static IEnumerable<(string mod, List<Warn> events)> common
//        {
//            get
//            {
//                return Mod.modDict.Select(x => (x.Key, x.Value.warnGroup._common));
//            }
//        }

//        internal static IEnumerable<(string mod, List<Warn> events)> depart
//        {
//            get
//            {
//                return Mod.modDict.Select(x => (x.Key, x.Value.warnGroup._depart));
//            }
//        }

//        internal static IEnumerable<(string mod, List<Warn> events)> pop
//        {
//            get
//            {
//                return Mod.modDict.Select(x => (x.Key, x.Value.warnGroup._pop));
//            }
//        }

//        internal static WarnGroup Load(string modname, string path)
//        {
//            return new WarnGroup()
//            {
//                _common = LoadSub(path + "/common"),
//                _depart = LoadSub(path + "/depart"),
//                _pop = LoadSub(path + "/pop")
//            };
//        }

//        internal static (string key, List<Desc> desc)[] Process()
//        {
//            var rslt = new List<(string key, List<Desc> desc)>();
//            foreach (var warn in common.SelectMany(x => x.events))
//            {
//                var datas = new List<string>();
//                if (warn.isValid())
//                {
//                    rslt.Add((warn.key, new List<Desc>() { warn.desc }));
//                }
//            }

//            //foreach (var warn in depart.SelectMany(x => x.events))
//            //{
//            //    var datas = new List<string>();
//            //    while (ModDataVisit.EnumerateVisit("depart"))
//            //    {
//            //        if (warn.isValid())
//            //        {
//            //            datas.Add((ModDataVisit.Get("depart.name") as string + "|" + warn.desc_str).TrimEnd('|'));
//            //        }
//            //    }

//            //    if (datas.Count != 0)
//            //    {
//            //        rslt.Add((warn.key, datas));
//            //    }
//            //}

//            return rslt.ToArray();
//        }

//        private static List<Warn> LoadSub(string path)
//        {
//            List<Warn> rslt = new List<Warn>();

//            if (!Directory.Exists(path))
//            {
//                return rslt;
//            }

//            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
//            {
//                var warnobj = ModElementLoader.Load<Warn>(file, File.ReadAllText(file));
//                warnobj.file = file;

//                rslt.Add(warnobj);
//            }

//            return rslt;
//        }
//    }
//}
