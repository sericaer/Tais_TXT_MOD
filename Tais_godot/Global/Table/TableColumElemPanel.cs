using System;
using Godot;

namespace TableGodot
{
    internal class TableColumElemPanel : PanelContainer
    {
        [Signal]
        public delegate void ORDER(string key, bool ascend);

        public const string path = "";

        internal string desc;

        bool ascend = false;

        Button button;

        internal static TableColumElemPanel Instance(Node parent, string key, string desc)
        {
            var panel = (TableColumElemPanel)ResourceLoader.Load<PackedScene>(path).Instance();
            panel.desc = desc;
            panel.Name = key;

            parent.AddChild(panel);
            return panel;
        }

        public override void _Ready()
        {
            button = GetNode<Button>("");
            button.Text = desc;
        }

        private void _on_Button_Pressed()
        {
            EmitSignal(nameof(ORDER), new object[] { Name, ascend });
            ascend = !ascend;
        }
    }
}