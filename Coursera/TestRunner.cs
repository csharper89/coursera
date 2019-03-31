using System;
using System.IO;
using System.Linq;

public class TestRunner
{
	public static void Run(string testDataPath, Func<string, string> testFunc)
	{
		var di = new DirectoryInfo(testDataPath).GetFiles();
		var input = di.Where(f => f.Name.StartsWith("input")).ToList();
		var output = di.Where(f => f.Name.StartsWith("output")).ToList();

		foreach (var i in input)
		{
			var result = testFunc(i.FullName);
			var correctResult = GetCorrectResult(i.FullName);
			var message = result == correctResult ? $"Test file {i.Name} passed" : $"Incorrect result for test file {i.Name}. Expected \"{correctResult}\", got \"{result}\"";
			Console.WriteLine(message);
		}
	}

	private static string GetCorrectResult(string inputFileName)
	{
		var outputFileName = inputFileName.Replace("input", "output");
		if(!File.Exists(outputFileName))
		{
			throw new Exception($"Could not find output file {outputFileName}");
		}

		return File.ReadAllText(outputFileName).Trim();
	}
}
