using Godot;
using System;

public class ButtonEx : Button
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var sub = GetChild<Control>(0);
		this.RectMinSize = new Vector2(sub.RectSize.x+10, sub.RectSize.y);

	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
