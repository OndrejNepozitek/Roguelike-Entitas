namespace Assets.Sources.Features.AI.SheepAndWolf
{
	public sealed class SheepAndWolfFeature : Feature
	{
		public SheepAndWolfFeature(Contexts contexts) : base("SheepAndWolf Systems")
		{
			Add(new SheepAiSystem(contexts));
			Add(new WolfAiSystem(contexts));
		}
	}
}