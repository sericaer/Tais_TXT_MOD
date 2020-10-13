using Godot;
using GMData.Run;

namespace TaisGodot.Scripts
{
	class DepartPanel : Panel
	{
		public Depart gmObj;

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


