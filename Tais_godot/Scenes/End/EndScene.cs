using Godot;

namespace TaisGodot.Scripts
{
	public class EndScene : Panel
	{
		public const string path = "res://Scenes/End/EndScene.tscn";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			

		}

		private void _on_Button_Confirm_button_up()
		{
			GetTree().ChangeScene(StartScene.path);
		}
	}
}
