using Godot;
using GMData.Run;

namespace TaisGodot.Scripts
{
	public class OutputPanel : HBoxContainer
	{
		internal OutputAdjust gmObj;

		HSlider slider;

		// Called when the node enters the scene tree for the first time.
		//public override void _Ready()
		//{
		//	slider = GetNode<HSlider>("HSlider");

		//	GetNode<Label>("Label").Text = gmObj.key;
		//	//GetNode<ReactiveLabel>("Value").Assoc(gmObj.currValue);

		//	slider.Value = gmObj.percent.Value;

		//	slider.Connect("mouse_entered", this, nameof(UpdateTooltip));
		//}

		//private void _on_HSlider_value_changed(float value)
		//{
		//	gmObj.percent.Value = value;
		//}

		//private void UpdateTooltip()
		//{
		//	slider.HintTooltip = $"{gmObj.percent.Value}%";
		//}
	}
}
