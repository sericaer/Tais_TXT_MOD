using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace TaisGodot.Scripts
{
	class WarnItem : Panel
	{
		public override void _Ready()
		{
			GetNode<Label>("Label").Text = Name + "_TITLE";
		}

		internal void Refresh(IEnumerable<GMData.Mod.Desc> descs)
		{

			var strInfo = String.Format("{0}\n-----------------\n{1}",
										TranslateServerEx.Translate(Name + "_TITLE"),
										String.Join("\n", descs.Select(x=>TranslateServerEx.Translate(x.Format, x.Params))));

			this.HintTooltip = strInfo;
		}
	}
}



