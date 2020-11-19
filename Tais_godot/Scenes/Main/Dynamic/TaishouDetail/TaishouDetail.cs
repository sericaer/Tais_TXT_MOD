using Godot;
using GMData.Run;
using GMData;
using System;

namespace TaisGodot.Scripts
{
	public class TaishouDetail : Panel
	{
		public const string path = "res://Scenes/Main/Dynamic/TaishouDetail/TaishouDetail.tscn";
		Label Name;
		Label Party;
		ReactiveLabel Age;

		internal static void Instance(Node parent)
		{
			var TaishouDetail = ResourceLoader.Load<PackedScene>(path).Instance();
			parent.AddChild(TaishouDetail);
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Name = GetNode<Label>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Name/Value");
			Party = GetNode<Label>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Party/Value");
			Age = GetNode<ReactiveLabel>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Age/Value");

			Name.Text = GMRoot.runner.taishou.name;
			Party.Text = GMRoot.runner.taishou.partyName;
			Age.Assoc(GMRoot.runner.taishou.OBSProperty(x=>x.age));
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
