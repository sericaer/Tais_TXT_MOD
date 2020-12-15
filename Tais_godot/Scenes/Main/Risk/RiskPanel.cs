using DynamicData;
using Godot;
using System;
using System.Linq;
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

		for (int i= 0; i < gmObj.unselectOpts.Count(); i++)
        {
			var elem = gmObj.unselectOpts.ElementAt(i);

			var btn = new Button();
			btn.Text = TranslateServerEx.Translate(elem.desc.format, elem.desc.argv);
			btn.Name = elem.desc.name;

			btn.Connect("pressed", this, nameof(_on_ButtonChoice_Pressed), new Godot.Collections.Array() { i});

		}

		foreach (var elem in gmObj.selectedChoices.Items)
		{
			OptionSelected(elem);
		}

		gmObj.selectedChoices.Connect().OnItemAdded(x=>OptionSelected(x)).Subscribe().EndWith(this);
	}

	private void _on_ButtonChoice_Pressed(int index)
    {
		gmObj.SelectChoice(index);
    }

    private void OptionSelected(string desc)
    {
		var opt = optContainer.GetChildren<Label>().Single(x => x.Name == desc);
		optContainer.RemoveChild(opt);

		var label = new Label();
		label.Text = TranslateServerEx.Translate(desc);
		label.Name = desc;

		vaildContainer.AddChild(label);
	}
}
