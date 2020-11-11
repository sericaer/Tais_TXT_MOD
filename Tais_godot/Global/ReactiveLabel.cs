using GMData.Run;
using Godot;
using System;

namespace TaisGodot.Scripts
{
	public class ReactiveProgressBar : ProgressBar
	{
		private IDisposable reactiveDispose;

		internal void Assoc(OBSValue<double> data)
		{
			reactiveDispose = data.Subscribe(this.SetProgressValue);
		}

		internal void Assoc(SubjectValue<double> data)
		{
			reactiveDispose = data.Subscribe(this.SetProgressValue);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			reactiveDispose?.Dispose();
		}


		private void SetProgressValue(double value)
		{
			Value = value;
		}
	}
}
