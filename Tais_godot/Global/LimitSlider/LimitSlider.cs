using System;
using Godot;

namespace TaisGodot.Scripts
{
	public class LimitSlider : Panel
	{
		[Signal]
		public delegate void ValueChanged(float value);

		public LimitSlider()
		{
		}

		public double Value
		{
			get
			{
				return LimitMinValue +(LimitMaxValue - LimitMinValue) * slider.Value / 100;
			}
			set
			{
				slider.Value = (value - LimitMinValue) * 100 / (LimitMaxValue - LimitMinValue);
			}
		}

		public float MinValue
		{
			get
			{
				return _MinValue;
			}
			set
			{
				if (_LimitMinValue != null && _LimitMinValue.Value > value)
				{
					_LimitMinValue = value;
				}

				_MinValue = value;
				UpdateSlider();
			}
		}

		public float MaxValue
		{
			get
			{
				return _MaxValue;
			}
			set
			{
				if (_LimitMaxValue != null && _LimitMaxValue.Value < value)
				{
					_LimitMaxValue = value;
				}

				_MaxValue = value;
				UpdateSlider();
			}
		}

		public float LimitMinValue
		{
			get
			{
				return _LimitMinValue != null? _LimitMinValue.Value : MinValue;
			}
			set
			{
				if(_LimitMinValue != null && _LimitMinValue.Value == value)
				{
					return;
				}

				if(value < MinValue)
				{
					throw new Exception($"LimitMinValue:{value} can not less than MinValue:{MinValue} in LimitSlider !");
				}
				if (value > LimitMaxValue)
				{
					throw new Exception($"LimitMinValue:{value} can not bigger than LimitMaxValue:{LimitMaxValue} in LimitSlider !");
				}

				_LimitMinValue = value;
				UpdateSlider();
			}
		}

		public float LimitMaxValue
		{
			get
			{
				return _LimitMaxValue != null ? _LimitMaxValue.Value : MaxValue;
			}
			set
			{
				if (_LimitMaxValue != null && _LimitMaxValue.Value == value)
				{
					return;
				}

				if (value > MaxValue)
				{
					throw new Exception($"LimitMaxValue:{value} can not bigger than MaxValue:{MaxValue} in LimitSlider !");
				}
				if (value < LimitMinValue)
				{
					throw new Exception($"LimitMaxValue:{value} can not less than LimitMinValue:{LimitMinValue} in LimitSlider !");
				}

				_LimitMaxValue = value;
				UpdateSlider();
			}
		}

		public override void _Ready()
		{
			slider = GetNode<Slider>("HSlider");

			_MinValue = (float)slider.MinValue;
			_MaxValue = (float)slider.MaxValue;

		}

		private void UpdateSlider()
		{
			var lenPercent = (LimitMaxValue - LimitMinValue) / (MaxValue - MinValue);
			var startPercent = (LimitMinValue - MinValue) / (MaxValue - MinValue);

			var newLen = this.RectSize.x * lenPercent;
			slider.RectSize = new Vector2(newLen, slider.RectSize.y);

			var newStart = this.RectSize.x * startPercent;

			if (newStart + slider.RectSize.x > this.RectSize.x)
			{
				newStart = this.RectSize.x - slider.RectSize.x;
			}

			
			slider.RectPosition = new Vector2(newStart, slider.RectPosition.y);
		}
		
		private void _on_HSlider_value_changed(float value)
		{
			EmitSignal(nameof(ValueChanged), Value);
		}

		private float _MinValue;
		private float _MaxValue;

		private float? _LimitMinValue;
		private float? _LimitMaxValue;

		private Slider slider;
	}
}
