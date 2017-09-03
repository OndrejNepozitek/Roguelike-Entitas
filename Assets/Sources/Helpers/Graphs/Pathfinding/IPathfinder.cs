namespace Assets.Sources.Helpers.Graphs.Pathfinding
{
	using System;
	using System.Collections.Generic;

	public interface IPathfinder<T>
	{
		IList<T> FindPath(IWeightedGraph<T> graph, T start, T end);

		IList<T> FindPath(IWeightedGraph<T> graph, T start, T end, Func<T, T, bool> goal);
	}
}
