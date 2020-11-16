using Godot;
using GMData.Run;
using GMData;
using System;

namespace TaisGodot.Scripts
{
	public class PopDetail : Panel
	{
		public const string path = "res://Scenes/Main/Dynamic/PopDetail/PopDetail.tscn";

		public GMData.Run.Pop gmObj;

		Label name;
		Node num;
		Node tax;
		Node farm;
		Node consume;
		Node adminSpend;

		internal static void Instance(Node parent, GMData.Run.Pop gmObj)
		{
			var panel = (PopDetail)ResourceLoader.Load<PackedScene>(path).Instance();
			panel.gmObj = gmObj;

			parent.AddChild(panel);
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			name = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/Name");

			num = GetNode("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/PopNum");
			tax = GetNode("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/Tax");
			farm = GetNode("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/Farm");
			consume = GetNode("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/Consume");
			adminSpend = GetNode("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/Admin");

			name.Text = gmObj.name;

			num.GetNode<ReactiveLabel>("Value").Assoc(gmObj.num);

			if (gmObj.tax != null)
			{
				tax.GetNode<ReactiveLabel>("Value").Assoc(gmObj.tax.OBSProperty(z => z.value));
			}
			if (gmObj.consume != null)
			{
				consume.GetNode<ReactiveLabel>("Value").Assoc(gmObj.consume.OBSProperty(z => z.value));
			}
			if (gmObj.adminExpend != null)
			{
				adminSpend.GetNode<ReactiveLabel>("Value").Assoc(gmObj.adminExpend.OBSProperty(z => z.value));
			}
			//farm.GetNode<ReactiveLabel>("Value").Assoc(gmObj.farm);     

		}
	}
}
