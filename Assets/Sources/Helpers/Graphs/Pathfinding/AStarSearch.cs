namespace Assets.Sources.Helpers.Graphs.Pathfinding
{
	using System;
	using System.Collections.Generic;
	using Heuristics;
	using Priority_Queue;

	public class AStarSearch<T> : IPathfinder<T>
	{
		public readonly IHeuristic<T> Heuristic;

		public AStarSearch(IHeuristic<T> heuristic)
		{
			Heuristic = heuristic;
		}

		protected bool DefaultGoal(T current, T end)
		{
			return current.Equals(end);
		}

		public IList<T> FindPath(IWeightedGraph<T> graph, T start, T end)
		{
			return FindPath(graph, start, end, DefaultGoal);
		}

		public IList<T> FindPath(IWeightedGraph<T> graph, T start, T end, Func<T, T, bool> goal)
		{
			var queue = new SimplePriorityQueue<T, double>();
			var cameFrom = new Dictionary<T, T>();
			var costSoFar = new Dictionary<T, double>();

			var isFound = false;
			T foundGoal = default(T);

			queue.Enqueue(start, 0);

			cameFrom[start] = start;
			costSoFar[start] = 0;

			while (queue.Count > 0)
			{
				var current = queue.Dequeue();

				if (goal(current, end))
				{
					foundGoal = current;
					isFound = true;
					break;
				}

				foreach (var next in graph.GetNeigbours(current))
				{
					var newCost = costSoFar[current] + graph.Cost(current, next);

					if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
					{
						costSoFar[next] = newCost;
						cameFrom[next] = current;

						var priority = newCost + Heuristic.Heuristic(next, end);
						queue.Enqueue(next, priority);
					}
				}
			}


			if (!isFound) return null;

			return ReconstructPath(cameFrom, start, foundGoal);
		}

		private IList<T> ReconstructPath(Dictionary<T, T> cameFrom, T start, T end)
		{
			var path = new List<T>();
			path.Add(end);
			var current = end;

			while (true)
			{
				var prev = cameFrom[current];

				if (prev.Equals(start))
				{
					break;
				}

				path.Add(prev);
				current = prev;
			}

			path.Reverse();

			return path;
		}
	}
}
