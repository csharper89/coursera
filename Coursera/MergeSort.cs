using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class Inversions
	{
		private long inversionsCount = 0;

		public int CountInversions(int[] arr)
		{
			inversionsCount = 0;
			SortAndCount(arr);

			return (int)inversionsCount;
		}

		private void OutputArray(IEnumerable<int> arr)
		{
			//Console.WriteLine(string.Join(" ", arr));
		}

		public int[] SortAndCount(int[] arr)
		{
			//Console.Write("SortAndCount is called: ");
			OutputArray(arr);
			if (arr.Length == 1)
			{
				return arr;
			}

			var half = arr.Length / 2;
			var a = arr.Take(half).ToArray();
			var b = arr.Skip(half).Take(arr.Length - half).ToArray();

			var aSorted = SortAndCount(a);
			var bSorted = SortAndCount(b);
			//Console.Write("aSorted: ");
			OutputArray(aSorted);
			//Console.Write("bSorted: ");
			OutputArray(bSorted);
			//if(aSorted[0] > bSorted[0])
			//{
			//	var tmp = bSorted;
			//	bSorted = aSorted;
			//	aSorted = tmp;
			//}

			var result = new int[arr.Length];
			var aIndex = 0;
			var bIndex = 0;
			for (var resultIndex = 0; resultIndex < result.Length; resultIndex++)
			{
				if (aSorted[aIndex] <= bSorted[bIndex])
				{
					result[resultIndex] = aSorted[aIndex++];
					if (aIndex >= aSorted.Length)
					{
						for (resultIndex++; resultIndex < result.Length; resultIndex++)
						{
							result[resultIndex] = bSorted[bIndex++];
						}
					}
				}
				else
				{
					if (aSorted[aIndex] > bSorted[bIndex])
					{
						inversionsCount += aSorted.Length - aIndex;
						//Console.WriteLine($"Inversion found. left: {aSorted[aIndex]}, right: {bSorted[bIndex]}");
					}

					result[resultIndex] = bSorted[bIndex++];
					if (bIndex >= bSorted.Length)
					{
						for (resultIndex++; resultIndex < result.Length; resultIndex++)
						{
							result[resultIndex] = aSorted[aIndex++];
						}
					}
				}
			}

			return result;
		}
	}
}
