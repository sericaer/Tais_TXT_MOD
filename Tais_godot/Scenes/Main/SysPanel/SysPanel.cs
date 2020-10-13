using Godot;

namespace TaisGodot.Scripts
{
	class SysPanel : Panel
	{
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
			GetTree().ChangeScene("res://Scenes/Start/StartScene.tscn");
		}

		private void _on_Button_Save_pressed()
		{
			var savePanel = (SaveLoadPanel)ResourceLoader.Load<PackedScene>("res://Global/SaveLoadPanel/SaveLoadPanel.tscn").Instance();
			savePanel.enableLoad = false;

			AddChild(savePanel);
		}
		
		private void _on_Button_Cancel_pressed()
		{
			QueueFree();
		}

	}
}
