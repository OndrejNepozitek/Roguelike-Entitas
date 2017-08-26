namespace Assets.Sources.Features.Loot.Systems
{
	using Entitas;
	using Helpers.Loot;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[InitializePhase(InitializePhase.RegisterDatabase)]
	public class RegisterLootSystem : IInitializeSystem
	{
		private readonly GameContext gameContext;

		public RegisterLootSystem(Contexts contexts)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			var loots = new LootDatabase();
			gameContext.AddService(loots);

			var globalGroup = new LootGroup();
			globalGroup.AddItem(ItemName.IronAxe, 1);
			globalGroup.AddItem(ItemName.Torch, 1);
			loots.RegisterItem(LootGroupName.Global, globalGroup);

			var basicChestGroup = new LootGroup();
			basicChestGroup.AddItem(ItemName.IronAxe, 1);
			loots.RegisterItem(LootGroupName.BasicChest, basicChestGroup);
		}
	}
}
