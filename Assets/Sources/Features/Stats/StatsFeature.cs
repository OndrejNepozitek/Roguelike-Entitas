namespace Assets.Sources.Features.Stats
{
	using Helpers.SystemDependencies;

	public class StatsFeature : Feature
	{
		public StatsFeature(Contexts contexts) : base("Stats feature")
		{
			Add(new ShouldDieSystem(contexts));
			Add(new StatsChangedSystem(contexts));
		}
	}
}