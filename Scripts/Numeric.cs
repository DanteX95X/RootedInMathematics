using System;
using Godot;

namespace RootedInMathematics.Scripts
{
	public class Numeric
	{
		public readonly double value;

		public Numeric(double value)
		{
			this.value = value;
		}

		public Numeric ToPower(double power)
		{
			return new Numeric(Math.Round(Math.Pow(value, power), 2 ));
		}
	}
}
