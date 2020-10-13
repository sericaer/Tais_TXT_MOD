using Godot;
using GMData.Run;

namespace TaisGodot.Scripts
{
	class PopPanel : Panel
	{
		internal Pop gmObj;

		public override void _Ready()
		{
			GetNode<Label>("Type").Text = gmObj.name;

			GetNode<ReactiveLabel>("Num").Assoc(gmObj.num);
		}
	}
}
