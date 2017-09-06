namespace Assets.Sources.Features.Stats
{
	using Systems;
	using Helpers.SystemDependencies;

	public class StatsFeature : Feature
	{
		public StatsFeature(Contexts contexts) : base("Stats feature")
		{
			Add(new ShouldDieSystem(contexts));
			Add(new StatsChangedSystem(contexts));
			Add(new RespawnPlayerSystem(contexts));
			Add(new RespawnPositionSystem(contexts));
		}
	}
}