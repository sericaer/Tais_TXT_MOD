using GMData;
using Godot;
using System;
using System.Linq;
using TaisGodot.Scripts;

public class EconomyDetailPanel2 : Godot.Panel
{
	public const string path = "res://Scenes/Main/Dynamic/EconomyDetail/EconomyDetailPanel2.tscn";

	ReactiveLabel surplus;
	ReactiveLabel incomeTotal;
	ReactiveLabel outputTotal;

	VBoxContainer adjustInputContainer;

	public static EconomyDetailPanel2 Instance(Node parent)
	{
		var panel = ResourceLoader.Load<PackedScene>(path).Instance();
		parent.AddChild(panel);

		return (EconomyDetailPanel2)panel;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		surplus = GetNode<ReactiveLabel>("CenterContainer/PanelContainer/AdjustContainer/AdjustSurplusContainer/MarginContainer/VBoxContainer/HBoxContainer/Value");
		incomeTotal = GetNode<ReactiveLabel>("CenterContainer/PanelContainer/AdjustContainer/AdjustInputContainer/MarginContainer/HBoxContainer/VBoxContainer/HBoxContainer/Value");
		outputTotal = GetNode<ReactiveLabel>("CenterContainer/PanelContainer/AdjustContainer/AdjustOutputContainer/MarginContainer/HBoxContainer/VBoxContainer/HBoxContainer/Value");

		surplus.Assoc(GMRoot.runner.economy.monthSurplus);
		incomeTotal.Assoc(GMRoot.runner.economy.incomeTotal);
		outputTotal.Assoc(GMRoot.runner.economy.outputTotal);

		adjustInputContainer = GetNode<VBoxContainer>("CenterContainer/PanelContainer/AdjustContainer/AdjustInputContainer/MarginContainer/HBoxContainer/VBoxContainer");

		foreach (GMData.Run.Adjust.EType elem in Enum.GetValues(typeof(GMData.Run.Adjust.EType)))
		{
			if(elem != GMData.Run.Adjust.EType.POP_TAX)
			{
				continue;
			}

			var adjustPanel = adjustInputContainer.GetNode<AdjustPanel>(elem.ToString());
			adjustPanel.Init(GMRoot.runner.adjusts.Single(x => x.etype == elem));
		}

	}



	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
