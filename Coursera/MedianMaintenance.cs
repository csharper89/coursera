using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class MedianMaintenance
	{
		public static List<int> GetRunningMegian(IEnumerable<int> numbers)
		{
			var res = new List<int>();
			for (var i = 0; i < numbers.Count(); i++)
			{
				var w = i + 1;
				var k = w % 2 == 0 ? (w / 2) : ((w + 1) / 2);
				var a = numbers.Take(w).OrderBy(e => e).ToList();
				res.Add(a[k - 1]);
			}
			return res;
		}

		public static List<int> GetRunningMegianHeap(List<int> numbers)
		{
			var hMax = new Heap<int>();
			var hLow = new Heap<int>(true);
			var res = new List<int>();

			for (var i = 0; i < numbers.Count; i++)
			{
				if (hMax.Size == 0 || numbers[i] > hMax.Peek())
				{
					hMax.Add(numbers[i]);
				}
				else
				{
					hLow.Add(numbers[i]);
				}
				if (hMax.Size > (hLow.Size + 1))
				{
					hLow.Add(hMax.Poll());
				}
				if (hLow.Size > (hMax.Size + 1))
				{
					hMax.Add(hLow.Poll());
				}

				var k = i + 1;
				var m = k / 2 + (k % 2 == 0 ? 0 : 1);
				if (hLow.Size == m)
				{
					res.Add(hLow.Peek());
				}
				else
				{
					res.Add(hMax.Peek());
				}
				
			}
			return res;
		}
	}

	public class MedianMaintenanceTest
	{
		public static void Test()
		{
			TestRunner.Run("TestData\\Median Maintanence", fileName =>
			{
				var loader = new TestDataLoader<int>(fileName);
				var testData = new List<int>();
				loader.Load(testData, int.Parse);
				var medians = MedianMaintenance.GetRunningMegianHeap(testData);
				return (medians.Sum() % 10000).ToString();
			});
		}
	}
}
