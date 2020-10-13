using Godot;
using GMData.Run;
using System;

namespace TaisGodot.Scripts
{
	class Risk : PanelContainer
	{
		internal GMData.Run.Risk gmObj;

		public override void _Ready()
		{
			GetNode<Label>("VBoxContainer/Label").Text = gmObj.key;
			GetNode<ReactiveProgressBar>("VBoxContainer/ProgressBar").Assoc(gmObj.percent);
		}
	}
}
