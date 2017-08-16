namespace Assets.Sources.Features.Items
{
	using Helpers.Entitas;

	public class ItemsFeature : Feature
	{
		public ItemsFeature(Contexts contexts) : base("Items feature")
		{
			Add(new EquipItemSystem(contexts));
			Add(new RegisterItemsSystem(contexts));
			Add(new SpawnItemSystem(contexts));
		}
	}
}