using System;
using Godot;

namespace TableGodot
{
    public class TabeDataElemUI : PanelContainer
    {
        public string desc;

        public Label label;

        public override void _Ready()
        {
            label = GetNode<Label>("");
            label.Text = desc;
        }
    }
}
