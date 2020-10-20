using System;
using GMData;
using Godot;

namespace TaisGodot.Scripts
{
	public class MainScene : Panel
	{
		public static string path = "res://Scenes/Main/MainScene.tscn";

		internal static MainScene inst;

		internal WarnContainer warnContainer;
		//internal TaskContainer taskContainer;
		internal RiskContainer riskContainer;

		public MainScene()
		{
			inst = this;
		}

		public override void _Ready()
		{
			//warnContainer = GetNode<WarnContainer>("VBoxContainer/WinContainer/ImpContainer/WarnContainer");
			//taskContainer = GetNode<TaskContainer>("VBoxContainer/WinContainer/TaskContainer");
			//riskContainer = GetNode<RiskContainer>("VBoxContainer/WinContainer/TaskContainer");
		}

		private async void _on_DaysInc()
		{
			GMRoot.runner.DaysInc();

			foreach (var spevent in SpecialEventDialog.Process())
			{
				await ToSignal(ShowSpecialDialog(spevent), "tree_exited");
			}

            foreach (var eventobj in GMRoot.modder.events)
            {
                await ToSignal(ShowDialog(eventobj), "tree_exited");
            }

            //warnContainer.Refresh(GMRoot.modder.warns);

            ////taskContainer.Refresh(Runner.GetTask());

            //riskContainer.Refresh(GMRoot.runner.risks);

            //if (GMRoot.runner.isEnd())
            //{
            //	GMRoot.runner = null;
            //	GetTree().ChangeScene("res://Scenes/End/EndScene.tscn");
            //}
        }

		internal static Node ShowDialog(GMData.Mod.GEvent eventobj)
		{
			return DialogPanel.Instance(inst, eventobj);
		}

		internal static Node ShowSpecialDialog(SpecialEventDialog spEvent)
		{
			return spEvent.Instance(inst);
		}

		private void _on_MapRect_MapClickSignal(int r, int g, int b)
		{
			GD.Print($"{r}, {g}, {b}");

			var depart = GMData.Run.Depart.GetByColor(r, g, b);
			if (depart == null)
			{
				return;
			}

			GD.Print($"select depart:{depart.name}");

			DepartPanel.Instance(this, depart);
		}

		private void OnEscSignal()
		{
			GMRoot.runner = null;
			GetTree().ChangeScene(StartScene.path);
		}

		private void _on_Button_Sys_pressed()
		{
			SysPanel.Instance(this);
		}

		private void _on_Button_Economy_pressed()
		{
			EconomyDetailPanel.Instance(this);
		}
		
		private void _on_ButtonChaoting_pressed()
		{
			ChaotingDetail.Instance(this);
		}
		
		private void _on_ButtonTaishou_pressed()
		{
			TaishouDetail.Instance(this);
		}
		
		private void _on_Button_RegistPop_pressed()
		{
			// Replace with function body.
		}
	}
}
