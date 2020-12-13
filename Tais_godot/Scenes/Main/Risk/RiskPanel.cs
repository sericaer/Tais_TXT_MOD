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
		
	}
}
