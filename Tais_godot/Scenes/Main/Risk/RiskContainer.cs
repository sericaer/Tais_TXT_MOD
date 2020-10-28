using GMData;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisGodot.Scripts
{
	class RiskContainer : HBoxContainer
	{
        internal IEnumerable<GMData.Mod.GEvent> Refresh()
        {
			var riskItems = this.GetChildren<Risk>().ToList();
			riskItems.RemoveAll(x => x.gmObj.isEnd);

			var needAdds = GMRoot.runner.risks.Where(x => !riskItems.Any(y => y.gmObj == x)).ToList();
			needAdds.ForEach(x =>
			{
				var riskItem = Risk.Instance(this, x);
				riskItem.Connect("tree_exited", this, nameof(_on_DeleteRisk), new Godot.Collections.Array() {x});
			});

			foreach(var risk in GMRoot.runner.risks)
            {
				if(risk.isEnd && risk.endEvent != null)
                {
					yield return GMRoot.modder.GetEvent(risk.endEvent);
                }
            }
		}

		private void _on_DeleteRisk(GMData.Run.Risk risk)
        {
			GMRoot.runner.risks.Remove(risk);
		}

        //internal List<GMData.Run.Risk> gmRisks;
    }
}
