using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursera
{
	public class Graph
	{
		List<Vertex> Vertices { get; set; }

		public (Vertex, Vertex) MinCut(List<Vertex> vertices)
		{
			Vertices = vertices;
			Contract();
			return (Vertices[0], Vertices[1]);
		}

		public int CountLinks(Vertex vertex1, Vertex vertex2)
		{
			int result = 0;

			var labels = vertex2.Fused.Select(f => f.Label).ToList();
			labels.Add(vertex2.Label);

			result += vertex1.ConnectedVertices.Count(v => labels.Contains(v));

			return result;
		}

		private void Contract()
		{
			if (Vertices.Count == 2)
			{
				return;
			}
			var now = DateTime.Now;
			var droppedTicks = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);


			var seed = DateTime.Now.Subtract(droppedTicks).Ticks;
			var r = new Random((int)seed);

			var index = r.Next(0, Vertices.Count - 1);
			//Console.WriteLine($"Seed: {seed} Index: {index}");
			var vertex = Vertices[index];
			var label1 = vertex.Label;

			var index1 = r.Next(0, vertex.ConnectedVertices.Count - 1);
			Fuse(label1, vertex.ConnectedVertices[index1]);
			Contract();
		}

		private void Fuse(string label1, string label2)
		{
			var vertex1 = Vertices.First(v => v.Label == label1);
			var vertex2 = Vertices.First(v => v.Label == label2);

			vertex1.Fused.AddRange(vertex2.Fused);
			vertex1.Fused.Add(vertex2);

			vertex1.ConnectedVertices.RemoveAll(l => l == vertex2.Label);
			vertex1.ConnectedVertices.AddRange(vertex2.ConnectedVertices.Where(l => l != vertex1.Label));
			Vertices.Remove(vertex2);
			foreach (var v in Vertices.Where(c => c.Label != vertex1.Label))
			{
				for(int i = 0; i < v.ConnectedVertices.Count; i++)
				{
					if (v.ConnectedVertices[i] == label2)
					{
						v.ConnectedVertices[i] = label1;
					}
				}				
			}
		}


	}

	public class Vertex
	{
		public List<Vertex> Fused { get; set; } = new List<Vertex>();

		public string Label { get; set; }

		public List<string> ConnectedVertices { get; set; }

		public override string ToString()
		{
			return $"{Label}   Connected: {string.Join(" ", ConnectedVertices)}   Fused: {string.Join(" ", Fused.Select(f => f.Label))}";
		}

		public static bool AreEqual(Vertex vertex1, Vertex vertex2)
		{
			if(vertex2.Fused.Count != vertex1.Fused.Count)
			{
				return false;
			}

			var labels1 = vertex2.Fused.Select(f => f.Label).ToList();
			labels1.Add(vertex2.Label);

			var labels2 = vertex1.Fused.Select(f => f.Label).ToList();
			labels2.Add(vertex1.Label);

			var result = labels1.All(l => labels2.Contains(l));
			return result;
		}
	}
}
