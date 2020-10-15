using Godot;

using GMData;
using System;

namespace TaisGodot.Scripts
{
	public class InitNameAgePanel : PanelContainer
	{
		public static string path = "res://Scenes/Init/Dynamic/InitNameAgePanel/InitNameAgePanel.tscn";

		[Signal]
		delegate void Finish();

		LineEdit nameEdit;
		Label ageLabel;
		Button btnAgeInc;
		Button btnAgeDec;

		const int MAX_AGE = 45;
		const int MIN_AGE = 25;
		const int DEF_AGE = 35;

		public override void _Ready()
		{
			nameEdit = GetNode<LineEdit>("VBoxContainer/NameContainer/LineEdit");
			ageLabel = GetNode<Label>("VBoxContainer/AgeContainer/Label");
			btnAgeInc = GetNode<Button>("VBoxContainer/AgeContainer/VBoxContainer/Button_Inc");
			btnAgeDec = GetNode<Button>("VBoxContainer/AgeContainer/VBoxContainer/Button_Dec");

			ageLabel.Text = DEF_AGE.ToString();

			_on_NameRandomButton_Pressed();
		}

		internal static InitNameAgePanel Instance()
		{
			return ResourceLoader.Load<PackedScene>(path).Instance() as InitNameAgePanel;
		}

		private void _on_ConfirmButton_Pressed()
		{
			GMRoot.initData.taishou.name = nameEdit.Text;
			GMRoot.initData.taishou.age = int.Parse(ageLabel.Text);

			Visible = false;

			EmitSignal(nameof(Finish));

			QueueFree();
		}

		private void _on_NameRandomButton_Pressed()
		{
			nameEdit.Text = GMRoot.define.personName.RandomFull;
		}

		private void _on_AgeIncButton_Pressed()
		{
			btnAgeDec.Disabled = false;

			int newAge = int.Parse(ageLabel.Text) + 1;
			if(newAge > MAX_AGE)
			{
				btnAgeInc.Disabled = true;
				return;
			}

			ageLabel.Text = newAge.ToString();
		}

		private void _on_AgeDecButton_Pressed()
		{
			btnAgeInc.Disabled = false;

			int newAge = int.Parse(ageLabel.Text) - 1;
			if (newAge < MIN_AGE)
			{
				btnAgeDec.Disabled = true;
				return;
			}

			ageLabel.Text = newAge.ToString();
		}
	}
}
