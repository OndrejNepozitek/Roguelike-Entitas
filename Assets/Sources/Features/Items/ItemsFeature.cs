namespace Assets.Sources.Features.Items
{
	using Systems;
	using Helpers.SystemDependencies;

	public class ItemsFeature : Feature
	{
		public ItemsFeature(Contexts contexts) : base("Items feature")
		{
			Add(new EquipItemSystem(contexts));
			Add(new RegisterItemsSystem(contexts));
			Add(new SpawnItemSystem(contexts));
			Add(new ItemsInputSystem(contexts));
			Add(new InventoryScreenInputSystem(contexts));
			Add(new InventoryUpdatedSystem(contexts));
		}
	}
}