using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class Scheduling
	{
		public static string GetSumOfCompletionTimes(List<Job> jobs)
		{
			var orderedByDifference = jobs.OrderByDescending(j => j.ScoreDifference).ThenByDescending(j => j.Weight);
			var orderedByRatio = jobs.OrderByDescending(j => j.ScoreRatio).ThenByDescending(j => j.Weight);
			return $"{GetSum(orderedByDifference)}\n{GetSum(orderedByRatio)}";			

			//return res; // 69119377652
		}

		private static long GetSum(IEnumerable<Job> orderedJobs)
		{
			var res = 0L;
			var completionTime = 0;
			foreach (var j in orderedJobs)
			{
				completionTime += j.Length;
				res += j.Weight * completionTime;
			}

			return res;
		}
	}

	public class Job
	{
		public int Weight { get; set; }

		public int Length { get; set; }

		public decimal ScoreDifference => Weight - Length;

		public decimal ScoreRatio => Weight / (decimal)Length;
	}

	public class SchedulingTest
	{
		public static void Test()
		{
			TestRunner.Run("TestData\\Scheduling", fileName =>
			{
				var jobs = new List<Job>();
				var loader = new TestDataLoader<Job>(fileName);
				loader.Load(jobs, s =>
				{
					var parts = s.Split(' ');
					return parts.Length == 2 ? new Job { Weight = int.Parse(parts[0]), Length = int.Parse(parts[1]) } : null;
				});
				return Scheduling.GetSumOfCompletionTimes(jobs).ToString();
			});
		}
	}
}
