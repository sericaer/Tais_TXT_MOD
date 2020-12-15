using DynamicData;
using Godot;
using System;
using TaisGodot.Scripts;

public class RiskPanel : Panel
{

	public const string path = "res://Scenes/Main/Risk/RiskPanel.tscn";

	private GMData.Run.Risk gmObj;

	private Container optContainer;
	private Container vaildContainer;
	private Label desc;
	private Label title;

	internal static void Instance(Control parent, GMData.Run.Risk gmObj)
	{
		var riskPanel = (RiskPanel)ResourceLoader.Load<PackedScene>(path).Instance();
		riskPanel.gmObj = gmObj;

		parent.AddChild(riskPanel);
	}

	public override void _Ready()
	{
		desc.Text = $"{gmObj.key}_DESC";
		title.Text = $"{gmObj.key}_TITLE";

		foreach(var elem in gmObj.unselectOpts)
        {
			var label = new Label();
			label.Text = TranslateServerEx.Translate(elem.desc.format, elem.desc.argv); ;
		}

		foreach (var elem in gmObj.selectedChoices.Items)
		{
			var label = new Label();
			label.Text = TranslateServerEx.Translate(elem);
		}

		gmObj.selectedChoices.Connect().OnItemAdded(x=>OptionSelected(x)).Subscribe().EndWith(this);
	}

    private void OptionSelected(string x)
    {
        throw new NotImplementedException();
    }
}
