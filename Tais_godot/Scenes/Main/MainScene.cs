using GMData;
using Godot;

namespace TaisGodot.Scripts
{
	public class MainScene : Panel
	{

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
			warnContainer = GetNode<WarnContainer>("VBoxContainer/WinContainer/ImpContainer/WarnContainer");
			//taskContainer = GetNode<TaskContainer>("VBoxContainer/WinContainer/TaskContainer");
			riskContainer = GetNode<RiskContainer>("VBoxContainer/WinContainer/TaskContainer");
		}

		private async void _on_DaysInc()
		{
			GMRoot.runner.DaysInc();

            foreach (var spevent in SpecialEventDialog.Process())
            {
                var dialog = ShowSpecialDialog(spevent);

                await ToSignal(dialog, "tree_exited");
            }

            foreach (var eventobj in GMRoot.modder.events)
            {
                var dialog = ShowDialog(eventobj);

                await ToSignal(dialog, "tree_exited");
            }

            warnContainer.Refresh(GMRoot.modder.warns);

            //taskContainer.Refresh(Runner.GetTask());

            riskContainer.Refresh(GMRoot.runner.risks);

            if (GMRoot.runner.isEnd())
            {
				GMRoot.runner = null;
				GetTree().ChangeScene("res://Scenes/End/EndScene.tscn");
            }
        }

        internal static Node ShowDialog(GMData.Mod.GEvent eventobj)
        {
            GD.Print(eventobj.title.Format);
            GD.Print(eventobj.desc.Format);
            foreach (var op in eventobj.options)
            {
                GD.Print(op.desc.Format);
            }

            var dialogNode = (DialogPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DialogPanel/DialogPanel.tscn").Instance();
            dialogNode.gEventObj = eventobj;

            inst.AddChild(dialogNode);

            return dialogNode;
        }

        internal static Node ShowSpecialDialog(SpecialEventDialog spEvent)
		{
			var dialogNode = (SpecialEventDialog)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DialogPanel/SpecialDialogPanel/" + spEvent.name + ".tscn").Instance();

			inst.AddChild(dialogNode);
			return dialogNode;
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

			var departNode = (DepartPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DepartPanel/DepartPanel.tscn").Instance();
			departNode.gmObj = depart;

			AddChild(departNode);
		}

		private void _on_Button_Cmd_button_up()
		{
			GetNode<Panel>("SysPanel").Visible = true;
		}

		private void OnEscSignal()
		{
			GMRoot.runner = null;
			GetTree().ChangeScene("res://Scenes/StartScene.tscn");
		}

		private void _on_Button_Sys_pressed()
		{
			var SysPanel = ResourceLoader.Load<PackedScene>("res://Scenes/Main/SysPanel/SysPanel.tscn").Instance();
			AddChild(SysPanel);
		}

		private void _on_Button_Economy_pressed()
		{
			var SysPanel = ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/EconomyDetail/EconomyDetailPanel.tscn").Instance();
			AddChild(SysPanel);
		}
		
		private void _on_ButtonChaoting_pressed()
		{
			var ChaotingDetail = ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/ChaotingDetail/ChaotingDetail.tscn").Instance();
			AddChild(ChaotingDetail);
		}
		
		private void _on_ButtonTaishou_pressed()
		{
			var TaishouDetail = ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/TaishouDetail/TaishouDetail.tscn").Instance();
			AddChild(TaishouDetail);
		}
	}
}
