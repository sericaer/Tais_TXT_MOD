using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Godot;

namespace TableGodot
{
    public class TableContainer : GridContainer
    {
        public List<(string key, string desc, string uiPath)> columnDef;

        public List<List<string>> tableData;

        public void InitColum()
        {
            var children = this.GetChildren<Node>();
            foreach(var child in children)
            {
                child.QueueFree();
            }

            this.Columns = columnDef.Count;

            for (int i = 0; i < columnDef.Count(); i++)
            {
                Node columElemPanel = TableColumElemPanel.Instance(this, columnDef[i].key, columnDef[i].desc);

                columElemPanel.Connect("ORDER", this, nameof(_on_OrderByColumn));
            }
        }

        public void SetDatas<T>(List<T> datas)
        {
            tableData = new List<List<string>>();

            Dictionary<string, FieldInfo> dictField = new Dictionary<string, FieldInfo>();

            foreach (var column in columnDef)
            {
                var field = typeof(T).GetField(column.key);
                if (field == null)
                {
                    throw new Exception();
                }

                dictField.Add(column.key, field);
            }

            foreach (var data in datas)
            {
                var row = new List<string>();
                for (int i=0; i< columnDef.Count; i++)
                {
                    var def = columnDef[i];

                    row.Add(dictField[def.key].GetValue(data).ToString());
                }

                tableData.Add(row);
            }

            UpdateTable();
        }

        private void UpdateTable()
        {
            var tabeDataElemPanel = this.GetChildren<TableDataElemUI>().ToList();
            tabeDataElemPanel.ForEach(x => x.QueueFree());

            for (int i=0; i< tableData.Count; i++)
            {
                for(int j=0; j< tableData[i].Count; j++)
                {
                    var data = tableData[i][j];
                    var def = columnDef[j];


                    Node child = LoadDataUI(def.uiPath, data);
                }
            }
        }

        private Node LoadDataUI(string path, string desc)
        {
            var tabeDataElemPanel = (TableDataElemUI)ResourceLoader.Load<PackedScene>(path).Instance();
            tabeDataElemPanel.desc = desc;

            AddChild(tabeDataElemPanel);

            return tabeDataElemPanel;
        }

        private void _on_OrderByColumn(string name, bool descend)
        {

            var index = columnDef.FindIndex(x => x.key == name);

            if(descend)
            {
                tableData.OrderByDescending(x => x[index]);
            }
            else
            {
                tableData.OrderBy(x => x[index]);
            }

            UpdateTable();
        }
    }

    public static class ObjectExtensions
    {
        public static IEnumerable<T> GetChildren<T>(this Node node) where T : Node
        {
            List<T> rslt = new List<T>();
            foreach (var child in node.GetChildren())
            {
                if (child is T)
                {
                    rslt.Add(child as T);
                }
            }

            return rslt;
        }
    }
}
