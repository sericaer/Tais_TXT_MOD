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

        public abstract bool IsVaild();

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
    }
}
