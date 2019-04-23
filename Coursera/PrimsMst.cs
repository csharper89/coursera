using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class PrimsMst
	{
		private Heap<DijkstraVertexInfo> _heap;

		private void Init(IEnumerable<VertexInfo> graph)
		{
			_heap = new Heap<DijkstraVertexInfo>(false, graph.Count());

			foreach (var vertex in graph)
			{
				_heap.Add(new DijkstraVertexInfo
				{
					Vertex = vertex
				});
			}

			_heap.Peek().MinDistance = 0;
		}

		public long GetMstCost(IEnumerable<VertexInfo> graph)
		{
			Init(graph);
			HashSet<VertexInfo> visited = new HashSet<VertexInfo>(graph.Count());
			var cost = 0L;

			while (_heap.Size > 0)
			{
				var vertex = _heap.Poll();
				cost += vertex.MinDistance;

				foreach (var c in vertex.Vertex.Connected)
				{
					var corresponding = _heap.Items.FirstOrDefault(v => v.Vertex.Label == c.Label);
					if (corresponding != null && corresponding.MinDistance > c.Distance)
					{
						var updated = new DijkstraVertexInfo
						{
							MinDistance = c.Distance,
							Vertex = corresponding.Vertex
						};
						_heap.Update(corresponding, updated);
					}
				}
			}
			return cost;
		}
	}

	class Edge
	{
		public int Source { get; set; }

		public int Destination { get; set; }

		public int Cost { get; set; }
	}

	public class PrimsMstTest
	{
		public static void Test()
		{
			TestRunner.Run("TestData\\PrimsMst", fileName =>
			{
				var data = new HashSet<Edge>();
				var loader = new TestDataLoader<Edge>(fileName);
				loader.Load(data, s =>
				{
					var parts = s.Split(' ');
					if (parts.Length == 2)
					{
						return null;
					}

					return new Edge
					{
						Source = int.Parse(parts[0]),
						Destination = int.Parse(parts[1]),
						Cost = int.Parse(parts[2]),
					};
				});
				var graph = new HashSet<VertexInfo>();
				foreach (var d in data)
				{
					graph.TryGetValue(new VertexInfo { Label = d.Source }, out var vertex);
					if (vertex == null)
					{
						vertex = new VertexInfo
						{
							Label = d.Source,
							Connected = new List<ConnectedVertex>()
						};
						graph.Add(vertex);
					}
					vertex.Connected.Add(new ConnectedVertex
					{
						Label = d.Destination,
						Distance = d.Cost
					});

					graph.TryGetValue(new VertexInfo { Label = d.Destination }, out var connected);
					if (connected == null)
					{
						connected = new VertexInfo
						{
							Label = d.Destination,
							Connected = new List<ConnectedVertex>()
						};
						graph.Add(connected);
					}
					if (!connected.Connected.Any(c => c.Label == d.Source))
					{
						connected.Connected.Add(new ConnectedVertex
						{
							Label = d.Source,
							Distance = d.Cost
						});
					}
				}

				var mst = new PrimsMst();
				var result = mst.GetMstCost(graph);
				return result.ToString();
			});
		}
	}
}

