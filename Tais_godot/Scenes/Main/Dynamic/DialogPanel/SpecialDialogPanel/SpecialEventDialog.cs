using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisGodot.Scripts
{
    internal abstract class SpecialEventDialog : Panel
    {
        private static List<SpecialEventDialog> all;

        static SpecialEventDialog()
        {
            all = new List<SpecialEventDialog>()
            {
                //new SelectCollectTaxLevel(),
                new ReportPopNumDialog(),
                //new ReportTaxDialog()
            };
        }

        public abstract string path { get; }

        public abstract bool IsVaild();

        internal string name
        {
            get
            {
                return GetType().Name;
            }
        }

        public override void _EnterTree()
        {
            SpeedContrl.Pause();
        }

        public override void _ExitTree()
        {
            SpeedContrl.UnPause();
        }

        internal static IEnumerable<SpecialEventDialog> Process()
        {
            foreach(var dialog in all)
            {
                if(dialog.IsVaild())
                {
                    yield return dialog;
                }
            }
        }

        internal static SpecialEventDialog GetEvent(string nextEventKey)
        {
            return all.SingleOrDefault(x => x.name == nextEventKey);
        }

        internal SpecialEventDialog Instance(Node parent)
        {
            var dialog = (SpecialEventDialog)ResourceLoader.Load<PackedScene>(path).Instance();
            parent.AddChild(dialog);

            return dialog;
        }
    }
}
