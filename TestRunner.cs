using System;
using System.IO;

public class TestRunner
{
	public void Run(string testDataPath, Func<string, string> testFunc)
	{
		var di = new DirectoryInfo(testDataPath).GetFiles();
		var input = di.Where(f => f.Name.StartsWith("input")).ToList();
		var output = di.Where(f => f.Name.StartsWith("output")).ToList();

		foreach (var i in input)
		{
			var result = testFunc(i.FullName);
		}
	}
}
