using GMData;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;

namespace TaisGodot.Scripts
{
	class RiskContainer : HBoxContainer
	{
		public override void _Ready()
		{
			GMRoot.runner._risks.Connect().OnItemAdded(_on_RiskObjAdd).Subscribe().EndWith(this);
			GMRoot.runner._risks.Connect().OnItemRemoved(_on_RiskObjRemove).Subscribe().EndWith(this);
		}

		private void _on_RiskObjAdd(GMData.Run.Risk risk)
		{
			Risk.Instance(this, risk);
		}

		private void _on_RiskObjRemove(GMData.Run.Risk risk)
		{
			var riskInst = this.GetChildren<Risk>().Single(x => x.gmObj == risk);
			this.RemoveChild(riskInst);
		}
	}
}
