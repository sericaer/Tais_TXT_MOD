using Godot;

namespace TaisGodot.Scripts
{
	class SysPanel : Panel
	{
		public const string path = "res://Scenes/Main/SysPanel/SysPanel.tscn";

		public static void Instance(Node parent)
		{
			var SysPanel = ResourceLoader.Load<PackedScene>(path).Instance();
			parent.AddChild(SysPanel);
		}

		public override void _EnterTree()
		{
			SpeedContrl.Pause();
		}

		public override void _ExitTree()
		{
			SpeedContrl.UnPause();
		}

		private void _on_Button_Quit_pressed()
		{
			GetTree().ChangeScene(StartScene.path);
		}

		private void _on_Button_Save_pressed()
		{
			SaveLoadPanel.Instance(this, false);
		}
		
		private void _on_Button_Cancel_pressed()
		{
			QueueFree();
		}

	}
}
