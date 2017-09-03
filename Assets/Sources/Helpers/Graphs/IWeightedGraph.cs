namespace Assets.Sources.Helpers.Graphs
{
	using System.Collections.Generic;

	public interface IWeightedGraph<T>
	{
		double Cost(T node1, T node2);

		IEnumerable<T> GetNeigbours(T node);
	}
}
