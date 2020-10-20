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

		internal static void Instance(Node parent, GMData.Run.Pop gmObj)
		{
			var panel = (PopDetail)ResourceLoader.Load<PackedScene>(path).Instance();
			panel.gmObj = gmObj;

			parent.AddChild(panel);
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			name = GetNode<Label>("");

			num = GetNode("");
			tax = GetNode("");
			farm = GetNode("");
			consume = GetNode("");

			name.Text = gmObj.name;

			num.GetNode<ReactiveLabel>("Value").Assoc(gmObj.num);
			tax.GetNode<ReactiveLabel>("Value").Assoc(gmObj.tax.value);
			//farm.GetNode<ReactiveLabel>("Value").Assoc(gmObj.farm);
			consume.GetNode<ReactiveLabel>("Value").Assoc(gmObj.consume.value);

		}
	}
}
