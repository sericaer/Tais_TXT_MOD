using Godot;
using GMData.Run;
using GMData;

namespace TaisGodot.Scripts
{
	public class TaishouDetail : Panel
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		Label Name;
		Label Party;
		ReactiveLabel Age;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Name = GetNode<Label>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Name/Value");
			Party = GetNode<Label>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Party/Value");
			Age = GetNode<ReactiveLabel>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Age/Value");

			Name.Text = GMRoot.runner.taishou.name;
			Party.Text = GMRoot.runner.taishou.party.name;
			Age.Assoc(GMRoot.runner.taishou.age);
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
