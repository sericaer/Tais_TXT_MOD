using GMData;
using Godot;
using System;

namespace TaisGodot.Scripts
{
	public class Date : HBoxContainer
	{
		public override void _Ready()
		{
			GetNode<ReactiveLabel>("Value").Assoc(GMRoot.runner.date.OBSProperty(x=>x.desc), 
												 (desc)=> TranslateServerEx.Translate("STATIC_DATE_VALUE", desc.Split('-')));
		}
	}

}
