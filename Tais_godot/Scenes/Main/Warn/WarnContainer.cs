using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace TaisGodot.Scripts
{
	class WarnContainer : HBoxContainer
	{
		public override void _Ready()
		{

		}

        internal void Refresh(IEnumerable<GMData.Mod.Warn> warns)
        {
			var warnItems = this.GetChildren<WarnItem>().ToList();

			var needRemoves = warnItems.FindAll(x => !warns.Any(y=>y.key == x.Name));
			needRemoves.ForEach(x =>
			{
				warnItems.Remove(x);
				x.QueueFree();
			});

			foreach(var warnGroup in warns.GroupBy(x=>x.key))
            {
				var warnItem = warnItems.SingleOrDefault(x => x.Name == warnGroup.Key);
				if (warnItem == null)
                {
					warnItem = (WarnItem)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Warn/WarnItem.tscn").Instance();
					warnItem.Name = warnGroup.Key;

					AddChild(warnItem);
				}

				warnItem.Refresh(warnGroup.Select(x=>x.desc));
			}
			
        }
	}
}



