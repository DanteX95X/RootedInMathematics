using System;
using System.Numerics;

namespace RootedInMathematics.Scripts
{
	public class Numeric
	{
		public readonly Complex complex;

		public Numeric(double real, double imaginary)
		{
			complex = new Complex(real, imaginary);
		}

		public Numeric ToPower(double power)
		{
			var otherComplex = Complex.Pow(complex, power);
			return new Numeric(Math.Round(otherComplex.Real, 2 ), Math.Round(otherComplex.Imaginary, 2));
		}

		public string ToString()
		{
			string result = complex.Real.ToString();
			if (complex.Imaginary != 0)
			{
				result += "+";
				result += complex.Imaginary.ToString();
				result += "i";
			}

			return result;
		}
	}
}
