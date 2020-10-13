//using Godot;
//using RunData;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace TaisGodot.Scripts
//{
//	class TaskContainer : VBoxContainer
//	{
//		internal void Refresh(IEnumerable<Process> processes)
//		{
//			var taskItems = this.GetChildren<Task>().ToList();

//			var needRemoves = taskItems.FindAll(x => !processes.Contains(x.gmObj));
//			needRemoves.ForEach(x =>
//			{
//				taskItems.Remove(x);
//				x.QueueFree();
//			});

//			var needAdds = processes.Where(x => !taskItems.Any(y => y.gmObj == x)).ToList();
//			needAdds.ForEach(x =>
//			{
//				var taskItem = (Task)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Task/Task.tscn").Instance();
//				taskItem.gmObj = x;

//				AddChild(taskItem);
//			});
//		}
//	}
//}
