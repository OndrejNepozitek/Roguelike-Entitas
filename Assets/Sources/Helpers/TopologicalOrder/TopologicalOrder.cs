namespace Assets.Sources.Helpers.TopologicalOrder
{
	using System;
	using System.Collections.Generic;

	public class TopologicalOrder<T>
	{
		private readonly List<Edge> edges = new List<Edge>();
		private readonly Dictionary<Type, Vertex> vertices = new Dictionary<Type, Vertex>();

		public void AddVertex(T item)
		{
			vertices.Add(item.GetType(), new Vertex() {Value = item});
		}

		public void AddEdge(Type from, Type to)
		{
			edges.Add(new Edge() { From = from, To = to });
		}
		
		public List<T> GetOrderedVertices()
		{
			ConstructGraph();

			var orderedVertices = new List<T>();
			var withoutIncoming = new Stack<Vertex>();

			foreach (var pair in vertices)
			{
				if (pair.Value.To.Count == 0)
				{
					withoutIncoming.Push(pair.Value);
				}
			}

			while (withoutIncoming.Count != 0)
			{
				var vertex = withoutIncoming.Pop();
				orderedVertices.Add(vertex.Value);

				foreach (var edge in vertex.From)
				{
					var toVertex = vertices[edge.To];
					toVertex.To.Remove(edge);

					if (toVertex.To.Count == 0)
					{
						withoutIncoming.Push(toVertex);
					}
				}
			}

			if (vertices.Count != orderedVertices.Count)
			{
				throw new ArgumentException("Given graph has at least one cycle");
			}

			return orderedVertices;
		}

		private void ConstructGraph()
		{
			foreach (var edge in edges)
			{
				var from = vertices[edge.From];
				var to = vertices[edge.To];

				from.From.Add(edge);
				to.To.Add(edge);
			}
		}

		private class Vertex
		{
			public T Value;
			public List<Edge> From = new List<Edge>();
			public List<Edge> To = new List<Edge>();
		}

		private class Edge
		{
			public Type From;
			public Type To;
		}
	}
}
