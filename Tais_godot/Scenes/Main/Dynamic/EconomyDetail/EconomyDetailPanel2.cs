using GMData;
using Godot;
using System;
using System.Linq;
using TaisGodot.Scripts;

using  RunnerAdjust = GMData.Run.Adjust;

public class EconomyDetailPanel2 : Godot.Panel
{
	public const string path = "res://Scenes/Main/Dynamic/EconomyDetail/EconomyDetailPanel2.tscn";

	ReactiveLabel surplus;
	ReactiveLabel incomeTotal;
	ReactiveLabel outputTotal;

	VBoxContainer adjustInputContainer;
	VBoxContainer adjustOutputContainer;

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
		adjustOutputContainer = GetNode<VBoxContainer>("CenterContainer/PanelContainer/AdjustContainer/AdjustOutputContainer/MarginContainer/HBoxContainer/VBoxContainer");

		var adjustEnumTypes = EnumEx.GetValues<RunnerAdjust.EType>();
		foreach (var elem in adjustEnumTypes.Where(x=>x.HasAttribute<RunnerAdjust.EconomyInput>()))
		{
			var adjustPanel = AdjustPanel.Instance(adjustInputContainer, GMRoot.runner.adjusts.Single(x => x.etype == elem));
		}

		foreach (var elem in adjustEnumTypes.Where(x => x.HasAttribute<RunnerAdjust.EconomyOutput>()))
		{
			var adjustPanel = AdjustPanel.Instance(adjustOutputContainer, GMRoot.runner.adjusts.Single(x => x.etype == elem));
		}
	}
}
