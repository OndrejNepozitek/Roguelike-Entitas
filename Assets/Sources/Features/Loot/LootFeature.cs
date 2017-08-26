namespace Assets.Sources.Features.Loot
{
	using Systems;
	using Helpers.SystemDependencies;

	public class LootFeature : Feature
	{
		public LootFeature(Contexts contexts) : base("Loot feature")
		{
			Add(new RegisterLootSystem(contexts));
			Add(new SpawnLootSystem(contexts));
			Add(new OpenChestInputSystem(contexts));
			Add(new OpenChestSystem(contexts));
		}
	}
}
