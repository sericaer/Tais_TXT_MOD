using GMData;
using Godot;
using System;
using System.Linq;

namespace TaisGodot.Scripts
{
	internal class ReportPopNumDialog : SpecialEventDialog
	{
		Label labelReal;
		Label labelReportLast;
		Label labelReportCurr;

		Slider sliderReportLast;
		Slider sliderReportCurr;

		public override string path => "res://Scenes/Main/Dynamic/DialogPanel/SpecialDialogPanel/ReportPopNumDialog.tscn";

		public ReportPopNumDialog()
		{

		}

		public override bool IsVaild()
		{
			return GMRoot.runner.date == (null, 8, 1);
		}

		public override void _Ready()
		{
			labelReal = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/RealPopNum/Value");
			labelReportLast = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/LastReport/Value");
			labelReportCurr = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/CurrLastReport/Value");

			sliderReportLast = GetNode<Slider>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/LastReport/HSlider");
			sliderReportCurr = GetNode<Slider>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/CurrLastReport/HSlider");

			labelReal.Text = GMRoot.runner.departs.Sum(x => x.popNum).ToString();
			labelReportLast.Text = GMRoot.runner.chaoting.reportPopNum.ToString();
			labelReportCurr.Text = labelReportLast.Text;

			sliderReportLast.MinValue = 0;
			sliderReportLast.MaxValue = 10;
			sliderReportLast.Value = 5;
			sliderReportLast.Rounded = true;

			sliderReportCurr.MinValue = 0;
			sliderReportCurr.MaxValue = 10;
			sliderReportCurr.Value = 5;
			sliderReportCurr.Rounded = true;

		}

		private void _on_SliderReportCurr_ValueChanged(double value)
		{
			var newReport = GMRoot.runner.chaoting.reportPopNum + GMRoot.runner.chaoting.reportPopNum * (sliderReportCurr.Value - 5) / 100;
			labelReportCurr.Text = ((int)newReport).ToString();
		}

		private void _on_ButtonConfrim_Pressed()
		{
			GMRoot.runner.chaoting.reportPopNum = int.Parse(labelReportCurr.Text);
			QueueFree();
		}
	}
}
