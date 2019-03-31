using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class SCCLocator
	{
		private HashSet<VertexInfo> visited = new HashSet<VertexInfo>();
		private Stack<VertexInfo> stack = new Stack<VertexInfo>();

		public List<SCC> GetSCCs(string fileName)
		{
			var sccs = new List<SCC>();
			var original = LoadFromFile(fileName);
			var reversed = LoadFromFile(fileName, true);

			foreach (var v in original)
			{
				if (!reversed.Contains(new VertexInfo { Label = v.Label }))
				{
					reversed.Add(new VertexInfo { Label = v.Label });
				}
			}

			foreach (var vertex in reversed)
			{
				Dfs(reversed, vertex);
			}

			while (stack.Count > 0)
			{
				var vertex = stack.Pop();
				if (!original.Contains(vertex))
				{
					continue;
				}

				original.TryGetValue(vertex, out vertex);
				if (!vertex.IsVisited)
				{
					var scc = new SCC();
					Dfs(original, vertex, scc.Components);
					sccs.Add(scc);
				}
			}

			return sccs.OrderByDescending(s => s.Components.Count).ToList();
		}

		private void Dfs(HashSet<VertexInfo> graph, VertexInfo vertex, List<VertexInfo> list = null)
		{
			if (vertex.IsVisited)
			{
				return;
			}

			var stack1 = new Stack<VertexInfo>();
			stack1.Push(vertex);

			while (stack1.Count > 0)
			{
				var ver = stack1.Pop();
				if (ver.IsVisited)
				{
					continue;
				}
				if (ver.Connected.Any(v => !v.IsVisited))
				{
					var connected = ver.Connected.First(v => !v.IsVisited);
					connected.IsVisited = true;
					stack1.Push(ver);
					var temp = new VertexInfo { Label = connected.Id };
					if (graph.Contains(temp))
					{
						graph.TryGetValue(temp, out temp);
						stack1.Push(temp);
					}
				}
				else if (stack1.Any())
				{
					if (list != null)
					{
						list.Add(ver);
					}
					else
					{
						stack.Push(ver);
					}
					ver.IsVisited = true;
				}
				else
				{
					if (list != null)
					{
						list.Add(ver);
					}
					ver.IsVisited = true;
				}
			}
		}

		private HashSet<VertexInfo> LoadFromFile(string filePath, bool reversed = false)
		{
			var graph = new HashSet<VertexInfo>();

			using (var fs = new FileStream(filePath, FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var s = "";
					while ((s = sr.ReadLine()) != null)
					{
						var parts = s.Split(' ');
						var info = new VertexInfo
						{
							Label = reversed ? int.Parse(parts[1]) : int.Parse(parts[0])
						};

						if (!graph.Contains(info))
						{
							graph.Add(info);
						}

						var connected = new ConnectedVertex
						{
							Id = reversed ? int.Parse(parts[0]) : int.Parse(parts[1]),
							IsVisited = false
						};
						info.Connected.Add(connected);
					}
				}
			}

			return graph;
		}
	}

	public class SCC
	{
		public List<VertexInfo> Components { get; set; } = new List<VertexInfo>();
	}

	public class ConnectedVertex
	{
		public int Id { get; set; }

		public bool IsVisited { get; set; }

		public int Distance { get; set; }
	}

	public class VertexInfo
	{
		public bool IsVisited { get; set; }

		public int Label { get; set; }

		public List<ConnectedVertex> Connected { get; set; } = new List<ConnectedVertex>();

		public override int GetHashCode()
		{
			return Label;
		}

		public override bool Equals(object obj)
		{
			var other = obj as VertexInfo;
			return other?.Label == Label;
		}

		public override string ToString()
		{
			return $"{Label} -> {string.Join(", ", Connected.Select(c => $"{c.Id}({c.Distance})"))}";
		}
	}
}
