namespace Assets.Sources.Features.FogOfWar
{
	using Helpers.Entitas;

	public sealed class FogOfWarFeature : Feature
	{
		public FogOfWarFeature(Contexts contexts) : base("FogOfWar feature")
		{
			Add(new AddFogSystem(contexts));
			Add(new RevealSystem(contexts));
			Add(new RemoveFogSystem(contexts));
		}
	}
}