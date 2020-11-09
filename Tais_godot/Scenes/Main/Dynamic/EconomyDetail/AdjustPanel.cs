using Godot;
using GMData.Run;
using System.Collections.Generic;
using System.Linq;

namespace TaisGodot.Scripts
{
	public class AdjustPanel : HBoxContainer
	{
        private Adjust gmObj;

        private HBoxContainer levelContainer;
        private ButtonGroup group;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
		{
            levelContainer = GetNode<HBoxContainer>("");
            group = new ButtonGroup();
        }

        internal void Init(Adjust gmObj)
        {
            this.gmObj = gmObj;

            var btn = levelContainer.GetNode<Button>("LEVEL1");

            for (int i = 0; i < gmObj.def.levels.Count; i++)
            {
                if (i > 0)
                {
                    btn = btn.Duplicate() as Button;
                    btn.Text = $"LEVEL{i + 1}";
                    levelContainer.AddChild(btn);
                }

                btn.Group = group;
                btn.HintTooltip = gmObj.def.levels[i].ToString();
                btn.Connect("pressed", this, nameof(_on_LevelButton_Pressed), new Godot.Collections.Array() {i+1});

            }

            gmObj.level.Subscribe(x =>
            {
                var currBtn = levelContainer.GetChild<Button>(x - 1);
                if (!currBtn.Pressed)
                {
                    currBtn.Pressed = true;
                }
            });

            gmObj.valid.Subscribe(x =>
            {
                foreach (Button elem in group.GetButtons())
                {
                    elem.Disabled = true;
                }
            });
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


