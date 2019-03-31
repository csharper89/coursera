using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class MergeSort
	{
		public static int[] Sort(int[] arr)
		{
			if(arr.Length == 1)
			{
				return arr;
			}

			var half = arr.Length / 2;
			var a = arr.Take(half).ToArray();
			var b = arr.Skip(half).Take(arr.Length - half).ToArray();

			var aSorted = Sort(a);
			var bSorted = Sort(b);

			var result = new int[arr.Length];
			var aIndex = 0;
			var bIndex = 0;
			for(var resultIndex = 0; resultIndex < result.Length; resultIndex++)
			{
				if(aSorted[aIndex] <= bSorted[bIndex])
				{
					result[resultIndex] = aSorted[aIndex++];
					if(aIndex >= aSorted.Length)
					{
						for(resultIndex++; resultIndex < result.Length; resultIndex++)
						{
							result[resultIndex] = bSorted[bIndex++];
						}
					}
				}
				else 
				{
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
