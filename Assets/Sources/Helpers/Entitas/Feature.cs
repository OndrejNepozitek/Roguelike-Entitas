namespace Assets.Sources.Helpers.Entitas
{
	using System.Collections.Generic;
	using global::Entitas;

	public class Feature
	{
		public List<ISystem> Systems = new List<ISystem>(); // TODO: could be better

		public Feature Add(ISystem system)
		{
			Systems.Add(system);
			return this;
		}

		public Feature()
		{
			
		}

		public Feature(string name)
		{
			
		}
	}
}
