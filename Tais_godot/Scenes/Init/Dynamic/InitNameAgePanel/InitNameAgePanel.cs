using Godot;

using GMData;
using System;

namespace TaisGodot.Scripts
{
    public class InitNameAgePanel : Panel
    {
        public static string path = "";

        [Signal]
        delegate void Finish();

        TextEdit nameEdit;
        Label ageLabel;
        Button btnAgeInc;
        Button btnAgeDec;

        const int MAX_AGE = 45;
        const int MIN_AGE = 25;
        const int DEF_AGE = 35;

        public override void _Ready()
        {
            nameEdit = GetNode<TextEdit>("");
            ageLabel = GetNode<Label>("");
            btnAgeInc = GetNode<Button>("");
            btnAgeDec = GetNode<Button>("");

            nameEdit.Text = GMRoot.define.personName.RandomFull;
            ageLabel.Text = DEF_AGE.ToString();
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
