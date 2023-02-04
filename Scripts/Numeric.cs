using Godot;

namespace RootedInMathematics.Scripts
{
	public struct Numeric
	{
		public readonly float value;

		public Numeric(float value)
		{
			this.value = value;
		}

		public Numeric ToPower(float power)
		{
			return new Numeric(Mathf.Pow(value, power));
		}
	}
}
