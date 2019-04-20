using System;
using System.Collections.Generic;
using System.Text;

namespace Coursera
{
	public class TwoSum
	{
		public int Calculate(HashSet<long> numbers, long from, long to)
		{
			var count = 0;

			for(var i = from; i <= to; i++)
			{
				if(ContainsSum(numbers, i))
				{
					count++;
				}
			}

			return count;
		}

		private bool ContainsSum(HashSet<long> numbers, long number)
		{
			foreach(var n in numbers)
			{
				var numberToLookFor = number - n;
				if(numbers.Contains(numberToLookFor))
				{
					return true;
				}
			}

			return false;
		}
	}

	public class TwoSumTest
	{
		public static void Test()
		{
			TestRunner.Run("TestData\\TwoSum", fileName =>
			{
				var data = new HashSet<long>();
				var loader = new TestDataLoader<long>(fileName);
				loader.Load(data, long.Parse);
				var twoSum = new TwoSum();
				return twoSum.Calculate(data, -10000, 10000).ToString();				
			});
		}
	}
}
