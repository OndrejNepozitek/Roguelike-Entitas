namespace Assets.Sources.Features.AI
{
	using SheepAndWolf;

	public sealed class AiFeature : Feature
	{
		public AiFeature(Contexts contexts) : base("AI Systems")
		{
			Add(new RandomAISystem(contexts));
			Add(new JumpAISystem(contexts));
			Add(new SheepAndWolfFeature(contexts));
		}
	}
}