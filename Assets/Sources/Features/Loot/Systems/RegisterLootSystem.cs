namespace Assets.Sources.Features.Loot.Systems
{
	using Entitas;
	using Helpers.Items;
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

			globalGroup.AddItem(ItemName.BronzeSword, 0.2f);
			globalGroup.AddItem(ItemName.LeatherArmor, 0.2f);

			globalGroup.AddItem(ItemName.BasicIronHelmet, 0.1f);
			globalGroup.AddItem(ItemName.SmallWoodenShield, 0.1f);
		

			loots.RegisterItem(LootGroupName.Global, globalGroup);

			var basicChestGroup = new LootGroup();

			basicChestGroup.AddItem(ItemName.SteelAxe, 0.2f);
			basicChestGroup.AddItem(ItemName.BigIronShield, 0.2f);
			basicChestGroup.AddItem(ItemName.RoundIronShield, 0.2f);
			basicChestGroup.AddItem(ItemName.DiamondSword, 0.2f);
			basicChestGroup.AddItem(ItemName.DiamondArmor, 0.2f);
			basicChestGroup.AddItem(ItemName.BlackArmor, 0.2f);
			basicChestGroup.AddItem(ItemName.CenturionHelmet, 0.0f);
			basicChestGroup.AddItem(ItemName.WingsHelmet, 0.2f);

			loots.RegisterItem(LootGroupName.BasicChest, basicChestGroup);
		}
	}
}
