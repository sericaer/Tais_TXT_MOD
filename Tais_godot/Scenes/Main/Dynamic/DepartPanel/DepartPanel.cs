using Godot;
using GMData.Run;
using System;

namespace TaisGodot.Scripts
{
	class DepartPanel : Panel
	{
		public const string path = "res://Scenes/Main/Dynamic/DepartPanel/DepartPanel.tscn";

		public Depart gmObj;


		internal static DepartPanel Instance(Node parent, Depart depart)
		{
			var panel = (DepartPanel)ResourceLoader.Load<PackedScene>(path).Instance();
			panel.gmObj = depart;

			parent.AddChild(panel);
			return panel;
		}

		public override void _Ready()
		{

			GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/Name").Text = gmObj.name;

			GetNode<ReactiveLabel>("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/PopNum/Value").Assoc(gmObj.popNum);
			GetNode<ReactiveLabel>("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/CropGrown/Value").Assoc(gmObj.cropGrown);

			GetNode<PopContainer>("CenterContainer/PanelContainer/VBoxContainer/PopContainer").SetPops(gmObj.pops);
		}

		private void _on_Button_button_up()
		{
			QueueFree();
		}
    }
}


