using System;
using System.Collections.Generic;
using GMData.Mod;
using Godot;

namespace TaisGodot.Scripts
{
	public class InitSelectPanel : PanelContainer
	{
		public static string path = "res://Scenes/Init/Dynamic/InitSelectPanel/InitSelectPanel.tscn";

		[Signal]
		public delegate void SelectNext(string next);

		internal InitSelect gmObj;

		internal RichTextLabel desc;
		internal VBoxContainer buttonContainer;

		internal static InitSelectPanel Instance()
		{
			return ResourceLoader.Load<PackedScene>(path).Instance() as InitSelectPanel;
		}

		public override void _Ready()
		{
			desc = GetNode<RichTextLabel>("VBoxContainer/RichTextLabel");
			buttonContainer = GetNode<VBoxContainer>("VBoxContainer/VBoxContainer");

			GD.Print("InitSelectPanel.30");

			desc.Text = TranslateServerEx.Translate(gmObj.desc.Format, gmObj.desc.Params);

			var buttons = CreateButton(gmObj.options.Length);

			for(int i=0; i<buttons.Count; i++)
			{
				var currBtn = buttons[i];
				var currOpt = gmObj.options[i];

				currBtn.Text = TranslateServerEx.Translate(currOpt.desc.Format, currOpt.desc.Params);
				currBtn.Connect("pressed", this, nameof(_on_Button_Pressed), new Godot.Collections.Array() { i });
			}
		}

		private List<Button> CreateButton(int count)
		{
			List<Button> buttons = new List<Button>();

			var btn = buttonContainer.GetChild<Button>(0);
			buttons.Add(btn);

			for (int i=1; i<count; i++)
			{
				var newBtn = btn.Duplicate() as Button;
				buttonContainer.AddChild(newBtn);
				buttons.Add(newBtn);
			}

			return buttons;
		}

		private void _on_Button_Pressed(int index)
		{
			Visible = false;

			gmObj.options[index].Selected();
			EmitSignal(nameof(SelectNext), gmObj.options[index].Next);

			QueueFree();
		}
	}
}
