using System;
using System.Collections.Generic;
using GMData.Mod;
using Godot;

namespace TaisGodot.Scripts
{
    public class InitSelectPanel : Panel
    {
        public static string path = "";

        [Signal]
        public delegate void SelectNext(string next);

        internal InitSelect gmObj;

        internal RichTextLabel desc;
        internal VBoxContainer buttonContainer;

        internal static InitSelectPanel Instance()
        {
            return ResourceLoader.Load<PackedScene>(path).Instance() as InitSelectPanel;
        }

        public override void _Ready()
        {
            desc = GetNode<RichTextLabel>("");
            buttonContainer = GetNode<VBoxContainer>("");

            desc.Text = TranslateServerEx.Translate(gmObj.desc.Format, gmObj.desc.Params);

            var buttons = CreateButton(gmObj.options.Length);

            for(int i=0; i<buttons.Count; i++)
            {
                var currBtn = buttons[i];
                var currOpt = gmObj.options[i];

                currBtn.Text = TranslateServerEx.Translate(currOpt.desc.Format, currOpt.desc.Params);
                currBtn.Connect("pressed", this, nameof(_on_Button_Pressed), new Godot.Collections.Array() { i });
            }
        }

        private List<Button> CreateButton(int count)
        {
            List<Button> buttons = new List<Button>();

            var btn = buttonContainer.GetChild<Button>(0);
            buttons.Add(btn);

            for (int i=1; i<count; i++)
            {
                var newBtn = btn.Duplicate() as Button;
                buttonContainer.AddChild(btn);
                buttons.Add(newBtn);
            }

            return buttons;
        }

        private void _on_Button_Pressed(int index)
        {
            Visible = false;

            gmObj.options[index].Selected();
            EmitSignal(nameof(SelectNext), gmObj.options[index].Next);

            QueueFree();
        }
    }
}
