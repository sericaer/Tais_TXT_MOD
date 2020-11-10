using Godot;
using GMData.Run;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TaisGodot.Scripts
{
	public class AdjustPanel : PanelContainer
	{
		private static AdjustPanel inst;

		private Adjust gmObj;

		private ButtonGroup group;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			group = new ButtonGroup();
		}

		public override void _EnterTree()
		{
			inst = this;
		}

		internal void Init(Adjust gmObj)
		{
			this.gmObj = gmObj;

			var levelContainer = GetNode<HBoxContainer>("HBoxContainer");
			var btn = levelContainer.GetNode<Button>("LEVEL1");

			for (int i = 0; i < gmObj.def.levels.Count; i++)
			{
				if (i > 0)
				{
					btn = btn.Duplicate() as Button;
					GetNode<HBoxContainer>("HBoxContainer").AddChild(btn);
				}
				btn.Text = $"STATIC_LEVEL{i + 1}";
				btn.Group = group;
				btn.HintTooltip = gmObj.def.levels[i].ToString();
				btn.Connect("pressed", this, nameof(_on_LevelButton_Pressed), new Godot.Collections.Array() { i + 1 });

			}

			gmObj.level.Subscribe(_on_LevelValue_Changed);

			gmObj.valid.Subscribe(x =>
			{
				foreach (Button elem in group.GetButtons())
				{
					elem.Disabled = !x;
				}
			});
		}

		private void _on_LevelValue_Changed(int level)
		{
			GD.Print(level);
			GD.Print(inst);
			var currBtn = inst.GetNode<HBoxContainer>("HBoxContainer").GetChild<Button>(level);
			if (!currBtn.Pressed)
			{
				currBtn.Pressed = true;
			}
		}

		private void _on_LevelButton_Pressed(int level)
		{
			GD.Print(level);
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


