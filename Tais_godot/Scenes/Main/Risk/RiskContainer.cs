using GMData;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisGodot.Scripts
{
	class RiskContainer : VBoxContainer
	{
        internal void Refresh()
        {
			var riskItems = this.GetChildren<Risk>().ToList();

			var needAdds = GMRoot.runner.risks.Where(x => !riskItems.Any(y => y.gmObj == x)).ToList();
			needAdds.ForEach(x =>
			{
				var riskItem = Risk.Instance(this, x);
				riskItem.Connect("tree_exited", this, nameof(_on_DeleteRisk), new Godot.Collections.Array() {x});
			});

			riskItems.RemoveAll(x => x.gmObj.isEnd);
		}

		private void _on_DeleteRisk(GMData.Run.Risk risk)
        {
			GMRoot.runner.risks.Remove(risk);
		}

        //internal List<GMData.Run.Risk> gmRisks;
    }
}
