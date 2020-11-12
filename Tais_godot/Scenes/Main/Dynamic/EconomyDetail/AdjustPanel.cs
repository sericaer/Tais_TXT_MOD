using Godot;
using GMData.Run;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TaisGodot.Scripts
{
	public class AdjustPanel : PanelContainer
	{
		public const string path = "res://Scenes/Main/Dynamic/EconomyDetail/AdjustPanel.tscn";

		internal Adjust gmObj;

		private ButtonGroup group = new ButtonGroup();

		internal static AdjustPanel Instance(Node parent, Adjust gmObj)
		{
			var panel = (AdjustPanel)ResourceLoader.Load<PackedScene>(path).Instance();
			panel.gmObj = gmObj;

			parent.AddChild(panel);

			return panel;
		}

		public override void _Ready()
		{
			var levelContainer = GetNode<HBoxContainer>("HBoxContainer");
			var btn = levelContainer.GetNode<Button>("LEVEL1");
			var label = levelContainer.GetNode<Label>("Label");

			label.Text = "STATIC_" + gmObj.etype.ToString();

			for (int i = 0; i < gmObj.def.levels.Count; i++)
			{
				if (i > 0)
				{
					btn = btn.Duplicate() as Button;
					GetNode<HBoxContainer>("HBoxContainer").AddChild(btn);
				}
				btn.Text = $"STATIC_LEVEL{i + 1}";
				btn.Group = group;
				btn.HintTooltip = GetLevelDesc(gmObj.etype, gmObj.def.levels[i]);
				btn.Connect("pressed", this, nameof(_on_LevelButton_Pressed), new Godot.Collections.Array() { i + 1 });

			}

			gmObj.level.Subscribe(x=>
			{
				GD.Print(x);
				var currBtn = GetNode<HBoxContainer>("HBoxContainer").GetChild<Button>(x);
				if (!currBtn.Pressed)
				{
					currBtn.Pressed = true;
				}
			}).EndWith(this);

			gmObj.valid.Subscribe(x =>
			{
				foreach (Button elem in group.GetButtons())
				{
					elem.Disabled = !x;
				}
			}).EndWith(this);
		}

		private void _on_LevelButton_Pressed(int level)
		{
			gmObj.level.Value = level;
		}

		private string GetLevelDesc(GMData.Run.Adjust.EType type, GMData.Def.Adjust.Level levelInfo)
		{
			var list = new List<(string desc, double percent)>();
			list.Add((type.ToString(), levelInfo.percent));

			if(levelInfo.effect_pop_consume != null)
			{
				list.Add(("EFFECT_POP_CONSUME", levelInfo.effect_pop_consume.Value));
			}

			return string.Join("\n", list.Select(x => $"{TranslateServerEx.Translate(x.desc)} {x.percent}%"));
		}
	}
}


