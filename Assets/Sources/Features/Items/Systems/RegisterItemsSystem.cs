namespace Assets.Sources.Features.Items.Systems
{
	using Entitas;
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

			items.RegisterItem(new Weapon(ItemName.IronAxe, Prefabs.IronAxe, 10));
			items.RegisterItem(new WeaponSecondary(ItemName.Torch, Prefabs.TorchHand));
		}
	}
}