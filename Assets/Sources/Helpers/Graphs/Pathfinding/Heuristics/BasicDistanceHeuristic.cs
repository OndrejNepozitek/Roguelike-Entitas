namespace Assets.Sources.Helpers.Graphs.Pathfinding.Heuristics
{
	using System;

	public class BasicDistanceHeuristic : IHeuristic<IntVector2>
	{
		public double Heuristic(IntVector2 node1, IntVector2 node2)
		{
			return Math.Abs(node1.X - node2.X) + Math.Abs(node1.Y - node2.Y);
		}
	}
}
