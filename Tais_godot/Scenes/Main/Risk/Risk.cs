using Godot;
using GMData.Run;
using System;

namespace TaisGodot.Scripts
{
	class Risk : PanelContainer
	{
		public const string path = "res://Scenes/Main/Risk/Risk.tscn";

		internal GMData.Run.Risk gmObj;

		internal static Risk Instance(Node parent, GMData.Run.Risk gmRisk)
		{
			var riskItem = (Risk)ResourceLoader.Load<PackedScene>(path).Instance();
			riskItem.gmObj = gmRisk;

			parent.AddChild(riskItem);
			return riskItem;
		}

		public override void _Ready()
		{
			GetNode<Label>("VBoxContainer/Label").Text = gmObj.key;
			GetNode<ReactiveProgressBar>("VBoxContainer/ProgressBar").Assoc(gmObj.percent);
		}


    }
}
