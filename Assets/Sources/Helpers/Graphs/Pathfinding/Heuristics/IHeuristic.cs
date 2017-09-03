namespace Assets.Sources.Helpers.Graphs.Pathfinding.Heuristics
{
	public interface IHeuristic<T>
	{
		double Heuristic(T node1, T node2);
	}
}
