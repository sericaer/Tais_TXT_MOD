using Godot;
using GMData;
using GMData.Mod;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace TaisGodot.Scripts
{
	public class StartScene : Panel
	{
		static StartScene()
		{
			GMRoot.logger = GD.Print;
			GMRoot.modder = new Modder(GlobalPath.mod);

			foreach (var language in GMRoot.modder.languages)
			{
				TranslateServerEx.AddTranslate(language);
			}

			Directory.CreateDirectory(GlobalPath.save);
		}

		private void _on_Button_Start_button_up()
		{
			GetTree().ChangeScene(InitScene.path);
		}

		private void _on_Button_Load_pressed()
		{
			var loadPanel = SaveLoadPanel.Instance(true);
			AddChild(loadPanel);

			loadPanel.Connect("LoadSaveFile", this, nameof(_on_LoadSaveFile_Signed));
		}

		private void _on_Button_Quit_pressed()
		{
			GetTree().Quit();
		}

		private void _on_LoadSaveFile_Signed(string fileName)
		{
			var content = File.ReadAllText(GlobalPath.save + fileName + ".save");
			GMRoot.runner = GMData.Run.Runner.Deserialize(content);

			GetTree().ChangeScene(MainScene.path);
		}
	}
}
