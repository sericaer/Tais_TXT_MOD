//using Godot;
//using System;
//using System.Linq;
//using RunData;

//namespace TaisGodot.Scripts
//{
//	internal class ReportTaxDialog : SpecialEventDialog
//	{
//		Label labelExpect;
//		Label labelReal;

//		LimitSlider sliderReal;


//		public ReportTaxDialog()
//		{
//		}

//		public override bool IsVaild()
//		{
//			return false;
//			//return COLLECT_POP_TAX.isFinishedDay();
//		}

//		public override void _Ready()
//		{
//			RunData.Economy.inst.curr.Value += COLLECT_POP_TAX.inst.collectedTax.Value;

//			labelExpect = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/ReportExpect/Value");
//			labelReal = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/ReportReal/Value");

//			sliderReal = GetNode<LimitSlider>("CenterContainer/PanelContainer/VBoxContainer/LimitSlider");

//			sliderReal.MinValue = 0;
//			sliderReal.MaxValue = (float)RunData.Economy.inst.curr.Value;

//			if (COLLECT_POP_TAX.inst.expectTax < RunData.Economy.inst.curr.Value)
//			{
//				sliderReal.LimitMinValue = (float)COLLECT_POP_TAX.inst.expectTax;
//			}
//			else
//			{
//				sliderReal.LimitMinValue = sliderReal.MaxValue;
//			}

//			sliderReal.Value = sliderReal.LimitMinValue;

//			labelExpect.Text = COLLECT_POP_TAX.inst.expectTax.ToString();
//			labelReal.Text = sliderReal.Value.ToString();

//		}

//		private void _on_SliderReportCurr_ValueChanged(float value)
//		{
//			labelReal.Text = value.ToString();
//		}

//		private void _on_ButtonConfrim_Pressed()
//		{
//			RunData.Chaoting.inst._extraTax += sliderReal.Value - COLLECT_POP_TAX.inst.expectTax;
//			RunData.Economy.inst.curr.Value -= sliderReal.Value;

//			GD.Print(RunData.Chaoting.inst._extraTax);
//			GD.Print(RunData.Chaoting.inst.oweTax);

//			QueueFree();
//		}
//	}
//}
