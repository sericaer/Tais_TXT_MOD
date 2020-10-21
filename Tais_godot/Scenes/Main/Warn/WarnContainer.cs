using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMData;
using Godot;

namespace TaisGodot.Scripts
{
	class WarnContainer : HBoxContainer
	{
		public override void _Ready()
		{

		}

        internal void Refresh()
        {
			var warnItems = this.GetChildren<WarnItem>().ToList();
			warnItems.ForEach(x => x.ClearDesc());

			foreach (var warn in GMRoot.modder.warns)
			{
				var warnItem = warnItems.SingleOrDefault(x => x.Name == warn.key);
				if (warnItem == null)
				{
					warnItem = (WarnItem)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Warn/WarnItem.tscn").Instance();
					warnItem.Name = warn.key;

					AddChild(warnItem);
				}

				warnItem.AddDesc(warn.desc);
			}

			warnItems.RemoveAll(x => x.DescEmpty());
		}
    }
}



