using Godot;
using GMData.Run;
using System;

namespace TaisGodot.Scripts
{
	public class ReactiveLabel : Label
	{
		private IDisposable reactiveDispose;

		internal void Assoc<T>(ObservableValue<T> data, Func<string, string> adpt = null)
		{
			this.adpt = adpt;
			reactiveDispose = data.Subscribe(this.SetValue);
		}

		internal void Assoc<T>(SubjectValue<T> data, Func<string, string> adpt = null)
		{
			this.adpt = adpt;
			reactiveDispose = data.Subscribe(this.SetValue);
		}

		internal void Assoc<T>(OBSValue<T> data, Func<string, string> adpt = null)
		{
			this.adpt = adpt;
			reactiveDispose = data.Subscribe(this.SetValue);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			reactiveDispose?.Dispose();
		}

		private Func<string, string> adpt;

		private void SetValue<T>(T value)
		{ 
			Text = adpt!=null ? adpt(value.ToString()) : value.ToString();
		}
	}
}
