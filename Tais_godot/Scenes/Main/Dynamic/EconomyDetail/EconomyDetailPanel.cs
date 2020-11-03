using Godot;
using GMData;
using System;

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

			foreach (var income in GMRoot.runner.economy.incomeAdjusts)
			{
				var incomPanel = (IncomePanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/EconomyDetail/IncomePanel.tscn").Instance();
				incomPanel.gmObj = income;

				GetNode<VBoxContainer>("CenterContainer/EconomyDetail/VBoxContainer/HBoxContainer/Income/VBoxContainer/VBoxContainer").AddChild(incomPanel);
			}

			foreach (var output in GMRoot.runner.economy.outputAdjusts)
			{
				var outputPanel = (OutputPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/EconomyDetail/OutputPanel.tscn").Instance();
				outputPanel.gmObj = output;

				GetNode<VBoxContainer>("CenterContainer/EconomyDetail/VBoxContainer/HBoxContainer/Output/VBoxContainer/VBoxContainer").AddChild(outputPanel);
			}

			////UpDateTempOutputStatus();
		}

		public override void _ExitTree()
		{
			SpeedContrl.UnPause();
		}

		private void _on_Button_Confirm_pressed()
		{
			QueueFree();
		}

		private void _on_Button_Cancel_pressed()
		{
			QueueFree();
		}

		private void _onButtonFullFillCountryTax()
		{
			//var need = RunData.Chaoting.inst.expectYearTax.Value- RunData.Chaoting.inst.realYearTax.Value;

			//RunData.Chaoting.inst.expectYearTax.Value = RunData.Chaoting.inst.realYearTax.Value;
			//RunData.Economy.inst.curr.Value -= need;
		}

		private void UpDateTempOutputStatus()
		{
			UpdateFullFillCountryTax();
		}

		private void UpdateFullFillCountryTax()
		{
			//var btn = GetNode<Button>("ButtonFullFillCountryTax");

			//var lackCountryTax = RunData.Chaoting.inst.expectYearTax.Value - RunData.Chaoting.inst.realYearTax.Value;
			//if (lackCountryTax >= 0)
			//{
			//	btn.Disabled = false;
			//	btn.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_NOT_LACK");
			//}
			//else if (RunData.Economy.inst.curr.Value < lackCountryTax)
			//{
			//	btn.Disabled = false;
			//	btn.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_LACK_AND_ECONOMY_NOT_SUFFICENT",
			//		lackCountryTax.ToString(),
			//		RunData.Economy.inst.curr.Value.ToString());
			//}
			//else
			//{
			//	btn.Disabled = true;
			//	btn.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_LACK_AND_ECONOMY_SUFFICENT",
			//		lackCountryTax.ToString(),
			//		RunData.Economy.inst.curr.Value.ToString());
			//}
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}

