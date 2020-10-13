using Godot;
using GMData;
using GMData.Mod;
namespace TaisGodot.Scripts
{
	public class StartScene : Panel
	{
		PackedScene mainScene;
		PackedScene saveLoadPanel;

		static StartScene()
		{
			System.IO.Directory.CreateDirectory(GlobalPath.save);

			GMRoot.logger = GD.Print;
			GMRoot.modder = new GMData.Mod.Modder(GlobalPath.mod);

			foreach (var language in GMRoot.modder.languages)
			{
				TranslateServerEx.AddTranslate(language);
			}
		}

		public override void _Ready()
		{
			mainScene = ResourceLoader.Load<PackedScene>("res://Scenes/Main/MainScene.tscn");
			saveLoadPanel = ResourceLoader.Load<PackedScene>("res://Global/SaveLoadPanel/SaveLoadPanel.tscn");
		}

		private void _on_Button_Start_button_up()
		{
			GMRoot.runner = new GMData.Run.Runner();

			GetTree().ChangeSceneTo(mainScene);
		}

		private void _on_Button_Load_pressed()
		{
			var loadPanel = saveLoadPanel.Instance() as SaveLoadPanel;
			loadPanel.enableLoad = true;

			AddChild(loadPanel);

			loadPanel.Connect("LoadSaveFile", this, nameof(_on_LoadSaveFile_Signed));
		}

		private void _on_Button_Quit_pressed()
		{
			GetTree().Quit();
		}

		private void _on_LoadSaveFile_Signed(string fileName)
		{
			var content = System.IO.File.ReadAllText(GlobalPath.save + fileName + ".save");
			GMRoot.runner = GMData.Run.Runner.Deserialize(content);

			GetTree().ChangeSceneTo(mainScene);
		}
	}
}
