using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisGodot.Scripts
{
	class RiskContainer : VBoxContainer
	{
        internal void Refresh(IEnumerable<GMData.Run.Risk> risks)
        {
			var riskItems = this.GetChildren<Risk>().ToList();

			var needRemoves = riskItems.FindAll(x => !risks.Contains(x.gmObj));
			needRemoves.ForEach(x =>
			{
				riskItems.Remove(x);
				x.QueueFree();
			});

			var needAdds = risks.Where(x => !riskItems.Any(y => y.gmObj == x)).ToList();
			needAdds.ForEach(x =>
			{
				var riskItem = (Risk)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Risk/Risk.tscn").Instance();
				riskItem.gmObj = x;

				AddChild(riskItem);
			});
		}
    }
}
