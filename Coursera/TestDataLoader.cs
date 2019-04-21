using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Coursera
{
	public class TestDataLoader<T>
	{
		private readonly string _fileName;

		public TestDataLoader(string fileName)
		{
			_fileName = fileName;
		}

		public void Load(ICollection<T> result, Func<string, T> parser)
		{
			using (var fs = new FileStream(_fileName, FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var s = "";
					while((s = sr.ReadLine()) != null)
					{
						if (!string.IsNullOrEmpty(s))
						{
							var parsed = parser(s.Trim());
							if (parsed != null)
							{
								result.Add(parsed);
							}
						}
					}
				}
			}
		}
	}	
}
