using Godot;
using System;
namespace TaisGodot.Scripts
{
	public class ChaotingDetail : Panel
	{
		public const string path = "res://Scenes/Main/Dynamic/ChaotingDetail/ChaotingDetail.tscn";
		Button btnFullFill;

		internal static void Instance(Node parent)
		{
			var panel = (ChaotingDetail)ResourceLoader.Load<PackedScene>(path).Instance();
			parent.AddChild(panel);
		}

		public override void _Ready()
		{
			GD.Print("_Ready");

			btnFullFill = GetNode<Button>("CenterContainer/PanelContainer/HBoxContainer/LeftContainer2/VBoxContainer/VBoxContainer/ButtonContainer/ButtonFullFill");
			btnFullFill.Connect("pressed", this, nameof(_onButtonFullFillCountryTax));


			//UpdateFullFillCountryTax();
		}

		//private void UpdateFullFillCountryTax()
		//{
		//	var lackCountryTax = RunData.Chaoting.inst.expectYearTax.Value - RunData.Chaoting.inst.realYearTax.Value;
		//	if (lackCountryTax == 0)
		//	{
		//		btnFullFill.Disabled = true;
		//		btnFullFill.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_NOT_LACK");
		//	}
		//	else if (RunData.Economy.inst.curr.Value < lackCountryTax)
		//	{
		//		btnFullFill.Disabled = true;
		//		btnFullFill.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_LACK_AND_ECONOMY_NOT_SUFFICENT",
		//			lackCountryTax.ToString(),
		//			RunData.Economy.inst.curr.Value.ToString());
		//	}
		//	else
		//	{
		//		btnFullFill.Disabled = false;
		//		btnFullFill.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_LACK_AND_ECONOMY_SUFFICENT",
		//			lackCountryTax.ToString(),
		//			RunData.Economy.inst.curr.Value.ToString());
		//	}
		//}

		private void _onButtonFullFillCountryTax()
		{
			//var need = RunData.Chaoting.inst.expectYearTax.Value - RunData.Chaoting.inst.realYearTax.Value;

			//RunData.Chaoting.inst.expectYearTax.Value = RunData.Chaoting.inst.realYearTax.Value;
			//RunData.Economy.inst.curr.Value -= need;

			//UpdateFullFillCountryTax();
		}

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }

}

