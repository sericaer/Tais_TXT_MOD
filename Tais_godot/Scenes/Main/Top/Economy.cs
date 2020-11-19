using GMData;
using Godot;
using System;
namespace TaisGodot.Scripts
{
	public class Economy : Button
	{
		public override void _Ready()
		{
			GetNode<ReactiveLabel>("HBoxContainer/Value").Assoc(GMRoot.runner.economy.OBSProperty(x => x.curr));
		}
	}
}
