namespace Assets.Sources.Features.Items
{
	using Entitas;

	/// <summary>
	/// Register all items to ItemDatabase.
	/// TODO: this could be done with a database file like xml or json
	/// </summary>
	public class RegisterItemsSystem : ISystem
	{
		// TODO: constructors should not be poluted for future config handling
		public RegisterItemsSystem(Contexts contexts)
		{
			var items = ItemDatabase.Instance;
			items.Reset();

			items.RegisterItem(new Weapon(ItemName.IronAxe, Prefabs.IronAxe, 10));
		}
	}
}