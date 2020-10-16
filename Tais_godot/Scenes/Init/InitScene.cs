using Godot;
using GMData;
using System.Linq;

namespace TaisGodot.Scripts
{
	public class InitScene : Panel
	{
		public static string path = "res://Scenes/Init/InitScene.tscn";

		CenterContainer initSelectContainer;

		public override void _Ready()
		{
			initSelectContainer = GetNode<CenterContainer>("CenterContainer");

			GMRoot.initData = new GMData.Init.InitData();

			var initNameAgePanel = InitNameAgePanel.Instance();
			initSelectContainer.AddChild(initNameAgePanel);

			initNameAgePanel.Connect("Finish", this, nameof(_on_SelectNameAgeFinish_Signal));
		}

		private void _on_SelectNameAgeFinish_Signal()
		{
			var firstInitSelect = GMRoot.modder.initSelects.Single(x => x.isFirst);

			CreateInitSelectPanel(firstInitSelect);
		}

		private void CreateInitSelectPanel(GMData.Mod.InitSelect initSelect)
		{
			var initSelectPanel = InitSelectPanel.Instance();
			GD.Print("initSelectPanel.gmObj = initSelect");
			initSelectPanel.gmObj = initSelect;

			initSelectPanel.Connect("SelectNext", this, nameof(_on_SelectNext_Signal));

			initSelectContainer.AddChild(initSelectPanel);
		}

		private void _on_SelectNext_Signal(string nextSelectName)
		{
			if(nextSelectName == "")
			{
				GMRoot.runner = new GMData.Run.Runner();
				GetTree().ChangeScene(MainScene.path);
				return;
			}

			var nextInitSelect = GMRoot.modder.initSelects.Single(x => x.key == nextSelectName);
			CreateInitSelectPanel(nextInitSelect);
		}
	}
}
