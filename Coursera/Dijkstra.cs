using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class Dijkstra
	{
		private Heap<DijkstraVertexInfo> _heap;

		public Dijkstra(HashSet<VertexInfo> graph)
		{
			_heap = new Heap<DijkstraVertexInfo>(false, graph.Count);

			foreach (var vertex in graph)
			{
				_heap.Add(new DijkstraVertexInfo
				{
					Vertex = vertex
				});
			}
		}

		private void InitializeTheHeap(VertexInfo start)
		{
			var original = new DijkstraVertexInfo
			{
				Vertex = start
			};
			var origin = new DijkstraVertexInfo
			{
				Vertex = start,
				MinDistance = 0,
				ShortestPathVia = start
			};
			_heap.Update(original, origin);
		}

		private List<VertexInfo> BuildShortestPath(int start, int destination, List<DijkstraVertexInfo> visited)
		{
			var result = new List<VertexInfo>();
			var current = visited.First(v => v.Vertex.Label == destination);
			while (current.Vertex.Label != start)
			{
				result.Add(current.Vertex);
				current = visited.First(v => v.Vertex.Label == current.ShortestPathVia.Label);
			}

			result.Add(current.Vertex);
			result.Reverse();
			return result;
		}

		private void UpdateConnectedVertex(DijkstraVertexInfo currentVertex, ConnectedVertex connected)
		{
			var corresponding = _heap.Items.FirstOrDefault(v => v.Vertex.Label == connected.Label);
			if (corresponding != null && (currentVertex.MinDistance + connected.Distance) < corresponding.MinDistance)
			{
				var updated = new DijkstraVertexInfo
				{
					MinDistance = currentVertex.MinDistance + connected.Distance,
					Vertex = corresponding.Vertex,
					ShortestPathVia = currentVertex.Vertex
				};
				_heap.Update(corresponding, updated);
			}
		}

		public List<VertexInfo> GetShortestPath(VertexInfo start, VertexInfo destination)
		{
			var visited = new List<DijkstraVertexInfo>();
			InitializeTheHeap(start);

			while (_heap.Size > 0)
			{
				var vertex = _heap.Poll();
				visited.Add(vertex);
				if (vertex.Vertex.Label == destination.Label)
				{
					return BuildShortestPath(start.Label, destination.Label, visited);
				}
				foreach (var connected in vertex.Vertex.Connected)
				{
					UpdateConnectedVertex(vertex, connected);
				}
			}

			return new List<VertexInfo>();
		}
	}

	public class DijkstraVertexInfo : IComparable<DijkstraVertexInfo>
	{
		public VertexInfo ShortestPathVia { get; set; }

		public int MinDistance { get; set; } = int.MaxValue;

		public VertexInfo Vertex { get; set; }

		public int CompareTo(DijkstraVertexInfo other)
		{
			return this.MinDistance == other.MinDistance ? 0 : this.MinDistance < other.MinDistance ? -1 : 1;
		}

		public override string ToString()
		{
			return $"{Vertex.Label}: Distance {MinDistance} via {ShortestPathVia?.Label}";
		}

		public override bool Equals(object obj)
		{
			var other = obj as DijkstraVertexInfo;
			if (other == null)
			{
				return false;
			}

			return other.Vertex.Label == this.Vertex.Label;
		}

		public override int GetHashCode()
		{
			return this.Vertex.Label;
		}
	}

	/// <summary>
	/// There are errors in the output files, so some tests fail. Checked by hand.
	/// </summary>
	public class DijkstraTest
	{
		public static int GetShortest(HashSet<VertexInfo> data, VertexInfo start, VertexInfo finish)
		{
			data.TryGetValue(start, out start);
			data.TryGetValue(finish, out finish);
			var sp = new Dijkstra(data);
			var path = sp.GetShortestPath(start, finish);

			var distance = 0;
			for (var i = 0; i < path.Count - 1; i++)
			{
				var next = path[i + 1];
				distance += path[i].Connected.First(c => c.Label == next.Label).Distance;
			}
			return distance;
		}

		public static void Test()
		{
			TestRunner.Run("TestData\\Dijkstra", fileName =>
			{
				var data = new HashSet<VertexInfo>();
				var loader = new TestDataLoader<VertexInfo>(fileName);
				loader.Load(data, s =>
				{
					var parts = s.Split('	');
					return new VertexInfo
					{
						Label = int.Parse(parts[0]),
						Connected = parts.Skip(1).Select(p =>
						{
							var edgeParts = p.Split(',');
							return new ConnectedVertex
							{
								Label = int.Parse(edgeParts[0]),
								Distance = int.Parse(edgeParts[1])
							};
						}).ToList()
					};
				});
				var start = new VertexInfo { Label = 1 };
				var distances = new List<int>();
				var indexes = new[] { 7, 37, 59, 82, 99, 115, 133, 165, 188, 197 };
				foreach (var i in indexes)
				{
					var finish = new VertexInfo { Label = i };
					distances.Add(GetShortest(data, start, finish));
				}
				return string.Join(",", distances);
			});
		}
	}
}
