using System;
using System.Collections.Generic;
using Godot;

namespace TaisGodot.Scripts
{
    [Tool]
    public class Table : HBoxContainer
    {
        private Dictionary<string, VBoxContainer> columDict;

        public Table()
        {
        }

        public void SetData(List<Dictionary<string, string>> list)
        {
            foreach(var elem in list)
            {
                foreach(var colum in columDict)
                {
                    var dataElement = (DataElement)ResourceLoader.Load<PackedScene>("res://Global/MsgboxPanel/MsgboxPanel.tscn").Instance();
                    colum.Value.AddChild(dataElement);

                    string value;
                    if (elem.TryGetValue(colum.Key, out value))
                    {
                        dataElement.Text = value;
                    }
                }
            }
        }
    }
}
