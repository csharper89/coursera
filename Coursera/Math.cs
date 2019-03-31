using System;
using System.Collections.Generic;
using System.Text;

namespace Coursera
{
	public class StringMath
	{
		public static string Add(string a, string b)
		{
			var padding = new string('0', Math.Abs(a.Length - b.Length));
			if (a.Length < b.Length)
			{
				a = padding + a;
			}
			else
			{
				b = padding + b;
			}

			var maxLength = Math.Max(a.Length, b.Length);
			var result = new char[maxLength + 1];
			var carry = 0;
			var resultIndex = result.Length - 1;
			for (var i = maxLength - 1; i >= 0; i--, resultIndex--)
			{
				var x = int.Parse(a[i].ToString());
				var y = int.Parse(b[i].ToString());
				var added = x + y + carry;
				carry = added / 10;
				result[resultIndex] = (added % 10).ToString()[0];
			}
			if (carry > 0)
			{
				result[resultIndex] = carry.ToString()[0];
			}
			return new string(result).TrimStart('\0');
		}

		public static string KaratsubaMultiply(string x, string y)
		{
			if (x.Length == 1 && y.Length == 1)
			{
				return (int.Parse(x) * int.Parse(y)).ToString();
			}

			var n = Math.Max(x.Length, y.Length);
			if (n % 2 != 0)
			{
				n++;
				if (x.Length > y.Length)
				{
					x = "0" + x;
				}
				else
				{
					y = "0" + y;
				}
			}
			var padding = new string('0', Math.Abs(x.Length - y.Length));
			if (x.Length < y.Length)
			{
				x = padding + x;
			}
			else
			{
				y = padding + y;
			}
			var half = x.Length / 2;
			var a = x.Substring(0, half);
			var b = x.Substring(half);
			var c = y.Substring(0, half);
			var d = y.Substring(half);

			var ac = KaratsubaMultiply(a, c);
			var ad = KaratsubaMultiply(a, d);
			var bc = KaratsubaMultiply(b, c);
			var bd = KaratsubaMultiply(b, d);
			var adPlusBc = Add(ad, bc);

			ac += new string('0', n);
			adPlusBc += new string('0', n / 2);

			return Add(ac, Add(adPlusBc, bd)).TrimStart('0');
		}
	}
}
