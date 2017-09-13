namespace Assets.Sources.Features.Items.Systems
{
	using Entitas;
	using Extensions;
	using Helpers;
	using Helpers.Items;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	/// <summary>
	/// Register all items to ItemDatabase.
	/// TODO: this could be done with a database file like xml or json
	/// </summary>
	[InitializePhase(InitializePhase.RegisterDatabase)]
	public class RegisterItemsSystem : IInitializeSystem
	{
		private readonly GameContext gameContext;

		public RegisterItemsSystem(Contexts contexts)
		{
			gameContext = contexts.game;
		}

		public void Initialize()
		{
			var items = new ItemDatabase();
			gameContext.AddService(items);

			items.RegisterItem(new Shield(ItemName.Torch, Prefabs.TorchHand, 25));

			// Weapons
			items.RegisterItem(new Weapon(ItemName.BronzeSword, Prefabs.BronzeSword, 10, 5));
			items.RegisterItem(new Weapon(ItemName.DiamondSword, Prefabs.BronzeSword, 15, 10));
			items.RegisterItem(new Weapon(ItemName.IronAxe, Prefabs.IronAxe, 15, -5));
			items.RegisterItem(new Weapon(ItemName.SteelAxe, Prefabs.SteelAxe, 20, -10));
			items.RegisterItem(new Weapon(ItemName.IronSpear, Prefabs.IronSpear, 10, criticalChance: 10));
			items.RegisterItem(new Weapon(ItemName.GoldAxe, Prefabs.SteelAxe, 5, criticalChance: 15));

			// Armor
			items.RegisterItem(new BodyArmor(ItemName.LeatherArmor, Prefabs.LeatherArmor, 10));
			items.RegisterItem(new BodyArmor(ItemName.DiamondArmor, Prefabs.DiamondArmor, 20, 10));
			items.RegisterItem(new BodyArmor(ItemName.BlackArmor, Prefabs.BlackArmor, 20, 20));

			// Helmets
			items.RegisterItem(new Helmet(ItemName.BasicIronHelmet, Prefabs.SmallHelmetIronRed, 10));
			items.RegisterItem(new Helmet(ItemName.CenturionHelmet, Prefabs.FullHelmetIronRed, 10, 10));
			items.RegisterItem(new Helmet(ItemName.WingsHelmet, Prefabs.WingsIronHelmet, 15, 10));

			// Shields
			items.RegisterItem(new Shield(ItemName.SmallWoodenShield, Prefabs.SmallWoodenShield, 10));
			items.RegisterItem(new Shield(ItemName.RoundIronShield, Prefabs.RoundIronShield, 15));
			items.RegisterItem(new Shield(ItemName.BigIronShield, Prefabs.IronShield, 20));
		}
	}
}