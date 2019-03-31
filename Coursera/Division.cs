using System;
using System.Collections.Generic;
using System.Text;

namespace Coursera
{
	public class Division
	{
		public static int MultiplyByRecursion(int x, int y)
		{
			if(x < 10 && y < 10)
			{
				return x * y;
			}

			var max = Math.Max(x, y);
			var n = ((int)Math.Log10(max) + 1) / 2;
			var powerOf10 = (int)Math.Pow(10, n);

			var a = x / powerOf10;
			var b = x % powerOf10;
			var c = y / powerOf10;
			var d = y % powerOf10;

			return (int)(Math.Pow(10, n * 2) * MultiplyByRecursion(a, c) + Math.Pow(10, n) * (MultiplyByRecursion(a, d) + MultiplyByRecursion(b, c)) + MultiplyByRecursion(b, d));
		}
	}
}
