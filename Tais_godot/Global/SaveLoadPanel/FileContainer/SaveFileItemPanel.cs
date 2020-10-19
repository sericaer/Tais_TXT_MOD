using System;
using Godot;

namespace TaisGodot.Scripts
{
	class SaveFileItemPanel : PanelContainer
	{
		public const string panelPath = "res://Global/SaveLoadPanel/FileContainer/SaveFileItem.tscn";

		internal string filePath;

        internal Action<string> actTriggerLoad;
		internal Action<string> actTriggerDelete;

		internal bool enableLoad;
		internal Button buttonLoad;
		internal Button buttonDelete;

        internal static SaveFileItemPanel Instance(Node parent, bool enableLoad, string filePath)
        {
			var panel = (SaveFileItemPanel)ResourceLoader.Load<PackedScene>(panelPath).Instance();
			panel.filePath = filePath;

			panel.buttonLoad.Visible = enableLoad;
			parent.AddChild(panel);

			return panel;
		}

        public override void _Ready()
		{
			GetNode<Label>("HBoxContainer/Label").Text = System.IO.Path.GetFileNameWithoutExtension(filePath);

			buttonLoad = GetNode<Button>("HBoxContainer/ButtonLoad");
			buttonDelete = GetNode<Button>("HBoxContainer/ButtonDelete");

			buttonLoad.Connect("pressed", this, nameof(on_LoadButton_Pressed));
			buttonDelete.Connect("pressed", this, nameof(on_DeleteButton_Pressed));
		}

		private void on_DeleteButton_Pressed()
        {
			actTriggerDelete(filePath);
			QueueFree();
        }

		private void on_LoadButton_Pressed()
		{
			actTriggerLoad(filePath);
		}
	}
}
