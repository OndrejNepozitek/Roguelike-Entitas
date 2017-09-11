namespace Assets.Sources.Features.ProcGen
{
	public sealed class ProcGenFeature : Helpers.SystemDependencies.Feature
	{
		public ProcGenFeature(Contexts contexts) : base("ProcGen Systems")
		{
			// Add(new RectangularMapSystem(contexts));
			Add(new FileMapSystem(contexts));
		}
	}
}