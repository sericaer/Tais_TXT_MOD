using GMData;
using Godot;
using System.Linq;
namespace TaisGodot.Scripts
{
	public class Pop : Button
	{
		public override void _Ready()
		{
			GetNode<ReactiveLabel>("HBoxContainer/Value").Assoc(GMRoot.runner.registerPopNum);
		}
	}
}
