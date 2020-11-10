using Godot;

using GMData;

using System;
using System.Linq;

namespace TaisGodot.Scripts
{
	public class EconomyDetailPanel : Panel
	{
		public const string path = "res://Scenes/Main/Dynamic/EconomyDetail/EconomyDetailPanel.tscn";
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		ReactiveLabel surplus;
		ReactiveLabel incomeTotal;
		ReactiveLabel outputTotal;

		VBoxContainer adjustContainer;

		public static EconomyDetailPanel Instance(Node parent)
		{
			var panel = ResourceLoader.Load<PackedScene>(path).Instance();
			parent.AddChild(panel);

			return (EconomyDetailPanel)panel;
		}

		public override void _Ready()
		{
			SpeedContrl.Pause();

			surplus = GetNode<ReactiveLabel>("CenterContainer/EconomyDetail/VBoxContainer/Bottom/Surplus/Value");
			incomeTotal = GetNode<ReactiveLabel>("CenterContainer/EconomyDetail/VBoxContainer/HBoxContainer/Income/VBoxContainer/Total/Value");
			outputTotal = GetNode<ReactiveLabel>("CenterContainer/EconomyDetail/VBoxContainer/HBoxContainer/Output/VBoxContainer/Total/Value");

			surplus.Assoc(GMRoot.runner.economy.monthSurplus);
			incomeTotal.Assoc(GMRoot.runner.economy.incomeTotal);
			outputTotal.Assoc(GMRoot.runner.economy.outputTotal);

			adjustContainer = GetNode<VBoxContainer>("");
			foreach(GMData.Run.Adjust.EType elem in Enum.GetValues(typeof(GMData.Run.Adjust.EType)))
			{
				var adjustPanel = adjustContainer.GetNode<AdjustPanel>(elem.ToString());
				adjustPanel.Init(GMRoot.runner.adjusts.Single(x => x.etype == elem));
			}
		}

		public override void _ExitTree()
		{
			SpeedContrl.UnPause();
		}
	}
}

